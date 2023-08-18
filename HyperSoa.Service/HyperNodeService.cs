using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.ActivityTracking.Monitors;
using HyperSoa.Service.ActivityTracking.Trackers;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.EventTracking;
using HyperSoa.Service.Extensions;
using HyperSoa.Service.Serialization;
using HyperSoa.Service.TaskIdProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HyperSoa.Service
{
    /// <summary>
    /// Processes <see cref="HyperNodeMessageRequest"/> objects and returns <see cref="HyperNodeMessageResponse"/> objects.
    /// This class is a singleton and must be referenced using the static <see cref="HyperNodeService.Instance"/> property.
    /// This class cannot be inherited.
    /// </summary>
    public sealed partial class HyperNodeService : IHyperNodeService, IDisposable
    {
        #region Defaults

        private static readonly IContractSerializer DefaultContractSerializer = new NoOpContractSerializer();
        private static readonly IHyperNodeConfigurationProvider DefaultConfigurationProvider = new InMemoryHyperNodeConfigurationProvider();

        #endregion Defaults

        #region Private Members

        private static readonly object Lock = new();

        private readonly TaskProgressCacheMonitor _taskProgressCacheMonitor = new();
        private readonly List<HyperNodeServiceActivityMonitor> _customActivityMonitors = new();
        private readonly ConcurrentDictionary<string, CommandModuleConfiguration> _commandModuleConfigurations = new();
        private readonly CancellationTokenSource _masterTokenSource = new();
        private readonly ConcurrentDictionary<string, HyperNodeTaskInfo> _liveTasks = new();

        #endregion Private Members

        #region Properties

        private string HyperNodeName { get; }

        internal bool EnableTaskProgressCache
        {
            get => _taskProgressCacheMonitor.Enabled;
            set => _taskProgressCacheMonitor.Enabled = value;
        }

        internal TimeSpan TaskProgressCacheDuration
        {
            get => _taskProgressCacheMonitor.CacheDuration;
            set => _taskProgressCacheMonitor.CacheDuration = value;
        }

        private IServiceProvider? ServiceProvider { get; init; }
        private ILogger<HyperNodeService> Logger { get; init; } = NullLogger<HyperNodeService>.Instance;
        private ITaskIdProvider TaskIdProvider { get; set; } = DefaultTaskIdProvider;
        private IHyperNodeEventHandler EventHandler { get; set; } = DefaultEventHandler;
        internal bool EnableDiagnostics { get; set; }
        internal int MaxConcurrentTasks { get; set; }

        /// <summary>
        /// Represents the singleton instance of the <see cref="HyperNodeService"/>.
        /// </summary>
        public static HyperNodeService Instance
        {
            get
            {
                if (_instance == null)
                    CreateAndConfigure(DefaultConfigurationProvider);

                return _instance ?? throw new HyperNodeConfigurationException("Unable to configure HyperNode service.");
            }
        }

        private static HyperNodeService? _instance;

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Processes and/or forwards the specified message.
        /// </summary>
        /// <param name="message">The <see cref="HyperNodeMessageRequest"/> object to process.</param>
        /// <returns></returns>
        public async Task<HyperNodeMessageResponse> ProcessMessageAsync(HyperNodeMessageRequest message)
        {
            Logger.LogTrace($"In {nameof(ProcessMessageAsync)}.");
            Logger.LogTrace($"{nameof(HyperNodeMessageRequest)} Values:");
            Logger.LogTrace("    {n,-19} = {v}", $"{nameof(message.CommandName)}", message.CommandName == null ? "<null>" : $"\"{message.CommandName}\"");
            Logger.LogTrace("    {n,-19} = {v}", $"{nameof(message.ProcessOptionFlags)}", message.ProcessOptionFlags);
            Logger.LogTrace("    {n,-19} = {v}", $"{nameof(message.CreatedByAgentName)}", message.CreatedByAgentName == null ? "<null>" : $"\"{message.CreatedByAgentName}\"");
            Logger.LogTrace("    {n,-19} = {v}", $"{nameof(message.CommandRequestBytes)}", message.CommandRequestBytes?.Length.ToString("Array (Count: #)") ?? "<null>");

            var response = new HyperNodeMessageResponse
            {
                RespondingNodeName = HyperNodeName,
                NodeAction = HyperNodeActionType.None,
                NodeActionReason = HyperNodeActionReasonType.Unknown,
                ProcessStatusFlags = MessageProcessStatusFlags.None
            };

            var currentTaskInfo = new HyperNodeTaskInfo(
                HyperNodeName,
                message,
                response,
                message.TaskTraceRequested() || EnableDiagnostics,
                _masterTokenSource.Token
            );

            // Start our task-level stopwatch to track total time. If diagnostics are disabled, calling this method has no effect.
            currentTaskInfo.StartStopwatch();

            // This tracker is used to temporarily store activity event items until we know what to do with them
            var queueTracker = new TaskActivityQueueTracker(currentTaskInfo);
            
            // We'll set the task trace a few different ways, but the logic determining whether it's returned in the response appears at the end of this method
            HyperNodeActivityItem[]? taskTrace = null;

            // Check if we should reject this request
            Logger.LogTrace("Analyzing request for acceptance or rejection.");
            var rejectionReason = GetRejectionReason(currentTaskInfo, queueTracker);
            if (rejectionReason.HasValue)
            {
                Logger.LogTrace("Rejecting request. Rejection reason: {r}", rejectionReason);
                response.NodeAction = HyperNodeActionType.Rejected;
                response.NodeActionReason = rejectionReason.Value;
                response.ProcessStatusFlags = MessageProcessStatusFlags.Cancelled;
                response.TaskId = null;

                // At this point in the game, we don't have a "real" activity tracker yet because we haven't initialized it. So instead of
                // running the events through the activity monitors as we normally would, we'll just add the information to the task trace
                // of the response object and send that back so the caller knows what happened.
                if (queueTracker.Count > 0)
                {
                    Logger.LogTrace("Constructing task trace for rejected request from queued activity items.");

                    taskTrace = queueTracker.Select(
                        activityItem =>
                        {
                            // These activity items would never be logged if they weren't logged here
                            Logger.LogTrace("{m}", activityItem.EventDescription);

                            return new HyperNodeActivityItem
                            {
                                Agent = activityItem.Agent,
                                EventDateTime = activityItem.EventDateTime,
                                EventDetail = activityItem.EventDetail,
                                EventDescription = activityItem.EventDescription,
                                ProgressPart = activityItem.ProgressPart,
                                ProgressTotal = activityItem.ProgressTotal,
                                Elapsed = activityItem.Elapsed
                            };
                        }
                    ).ToArray();
                }

                // If the message was rejected, then the HyperNodeTaskInfo was not added to the list of live events, which means it will never be disposed unless we do so here.
                // This will also set the TotalRunTime on the response so we know how long it took to reject the message
                currentTaskInfo.Dispose();
            }
            else
            {
                var taskTraceMonitor = new TaskTraceMonitor();

                // This node accepts responsibility for processing this message
                response.NodeAction = HyperNodeActionType.Accepted;
                response.NodeActionReason = HyperNodeActionReasonType.ValidMessage;
                Logger.LogTrace("Request accepted.");

                // Initialize our activity tracker so we can track progress
                Logger.LogTrace("Initializing activity tracker.");
                InitializeActivityTracker(currentTaskInfo, taskTraceMonitor);

                // Now that we've setup our "real" activity tracker, let's report all the activity we've stored up until now
                while (queueTracker.Count > 0)
                {
                    // If the item doesn't have a task ID, set it here.
                    var activityItem = queueTracker.Dequeue();
                    if (string.IsNullOrWhiteSpace(activityItem.TaskId))
                        activityItem.TaskId = currentTaskInfo.TaskId;

                    // Now track it verbatim (all info is preserved, i.e. the original date/time of the event and other properties)
                    currentTaskInfo.Activity?.TrackActivityVerbatim(activityItem);
                }

                #region Process Message

                try
                {
                    if (currentTaskInfo.TaskId != null)
                    {
                        currentTaskInfo.BeginTaskIdLogScope(
                            Logger.BeginScope(currentTaskInfo.TaskId)
                        );
                    }

                    // Track that we've started a task to process the message.
                    currentTaskInfo.Activity?.TrackTaskStarted();

                    // Check if user cancelled the message in the OnTaskStarted event
                    currentTaskInfo.Token.ThrowIfCancellationRequested();
                    
                    currentTaskInfo.Activity?.Track("Attempting to process message...");

                    // Define the method in a safe way (i.e. with a try/catch around it)
                    var processMessageInternalSafe = new Func<HyperNodeTaskInfo, Task>(
                        async args =>
                        {
                            try
                            {
                                await ProcessMessageInternal(args).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                if (ex is OperationCanceledException)
                                    args.Response.ProcessStatusFlags |= MessageProcessStatusFlags.Cancelled;
                                else
                                    args.Response.ProcessStatusFlags = MessageProcessStatusFlags.Failure;

                                if (ex is InvalidCommandRequestTypeException)
                                    args.Response.ProcessStatusFlags |= MessageProcessStatusFlags.InvalidCommandRequest;

                                args.Activity?.TrackException(ex);
                            }
                            finally
                            {
                                args.Activity?.TrackMessageProcessed();
                            }
                        }
                    );

                    if (message.ConcurrentRunRequested())
                    {
                        currentTaskInfo.AddChildTask(
                            Task.Run(
                                async () => await processMessageInternalSafe(currentTaskInfo).ConfigureAwait(false),
                                currentTaskInfo.Token
                            )
                        );
                    }
                    else
                    {
                        await processMessageInternalSafe(currentTaskInfo).ConfigureAwait(false);
                    }

                    // Now that we have all of our tasks doing stuff, we need to make sure we clean up after ourselves.
                    if (message.ConcurrentRunRequested())
                    {
                        // If we're running concurrently, we want to return immediately and allow the clean up to occur as a continuation after the tasks are finished.
                        // IMPORTANT: We're deliberately not using await here because we *want* to return asap because the user requested the command be run concurrently.
                        _ = currentTaskInfo.WhenChildTasks().ContinueWith(
                            _ =>
                            {
                                TaskCleanUp(currentTaskInfo.TaskId);

                                // Set our response task trace here (if applicable) so it's available in case the response lives in the progress cache.
                                if (currentTaskInfo.Message.TaskTraceRequested())
                                    response.TaskTrace = taskTraceMonitor.GetTaskTrace();
                            }
                        );
                    }
                    else
                    {
                        // Otherwise, if we're running synchronously, we want to block until all our child threads are done, then clean up.
                        currentTaskInfo.WaitChildTasks(_masterTokenSource.Token);
                        TaskCleanUp(currentTaskInfo.TaskId);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException)
                        response.ProcessStatusFlags |= MessageProcessStatusFlags.Cancelled;
                    else
                        response.ProcessStatusFlags = MessageProcessStatusFlags.Failure;

                    currentTaskInfo.Activity?.TrackException(ex);

                    // Make sure we clean up our task if any exceptions were thrown
                    TaskCleanUp(currentTaskInfo.TaskId);
                }

                // Wait until the very end to set this array to make sure we snag every activity item
                taskTrace = taskTraceMonitor.GetTaskTrace();

                #endregion Process Message
            }

            // Immediate responses for async tasks don't get task traces (they'll be added later when the async task completes).
            if (currentTaskInfo.Message.TaskTraceRequested() && !currentTaskInfo.Message.ConcurrentRunRequested())
                response.TaskTrace = taskTrace;

            Logger.LogTrace($"Returning from {nameof(ProcessMessageAsync)}.");
            return response;
        }

        /// <summary>
        /// Initiates a cancellation request.
        /// </summary>
        public void Cancel()
        {
            if (!_masterTokenSource.IsCancellationRequested)
                _masterTokenSource.Cancel();
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        public void WaitAllChildTasks()
        {
            Task.WaitAll(GetChildTasks().ToArray());
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the tasks to complete.</param>
        public void WaitAllChildTasks(CancellationToken token)
        {
            Task.WaitAll(GetChildTasks().ToArray(), token);
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.</param>
        public void WaitAllChildTasks(TimeSpan timeout)
        {
            Task.WaitAll(GetChildTasks().ToArray(), timeout);
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        public void WaitAllChildTasks(int millisecondsTimeout)
        {
            Task.WaitAll(GetChildTasks().ToArray(), millisecondsTimeout);
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the tasks to complete.</param>
        public void WaitAllChildTasks(int millisecondsTimeout, CancellationToken token)
        {
            Task.WaitAll(GetChildTasks().ToArray(), millisecondsTimeout, token);
        }

        /// <summary>
        /// Releases disposable resources consumed by this <see cref="HyperNodeService"/> instance.
        /// </summary>
        public void Dispose()
        {
            // Dispose of our task info objects
            foreach (var taskInfo in _liveTasks.Values)
            {
                taskInfo.Dispose();
            }

            // Dispose of any of our activity monitors that implement IDisposable
            foreach (var disposableMonitor in _customActivityMonitors.OfType<IDisposable>())
            {
                disposableMonitor.Dispose();
            }

            // Check if our ITaskIdProvider needs to be disposed
            (TaskIdProvider as IDisposable)?.Dispose();

            // Dispose of our task progress cache monitor
            _taskProgressCacheMonitor.Dispose();

            // Dispose of our master cancellation token source
            _masterTokenSource.Dispose();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initializes an instance of the <see cref="HyperNodeService"/> class with the specified name.
        /// </summary>
        /// <param name="hyperNodeName">The name of the <see cref="HyperNodeService"/>.</param>
        private HyperNodeService(string hyperNodeName)
        {
            HyperNodeName = hyperNodeName;
        }

        private void InitializeActivityTracker(HyperNodeTaskInfo currentTaskInfo, TaskTraceMonitor taskTraceMonitor)
        {
            currentTaskInfo.Activity = new HyperNodeTaskActivityTracker(
                currentTaskInfo,
                EventHandler,

                // Possible user actions
                currentTaskInfo.Cancel,
                Logger
            );

            try
            {
                // Create our activity feed by bridging the event tracker with reactive extensions
                var liveEvents = Observable.FromEventPattern<TrackActivityEventHandler, TrackActivityEventArgs>(
                    h => currentTaskInfo.Activity.TrackActivityHandler += h,
                    h => currentTaskInfo.Activity.TrackActivityHandler -= h
                ).Select(
                    a => (IHyperNodeActivityEventItem)a.EventArgs.ActivityItem // Cast all our activity items as IHyperNodeActivityEventItem
                );

                var systemActivityMonitors = new List<HyperNodeServiceActivityMonitor>();
                /*****************************************************************************************************************
                 * Subscribe our task progress cache monitor to our event stream only if the client requested it and the feature is actually enabled. There is currently no built-in
                 * functionality to support long-running task tracing other than the memory cache. If the client opts to disable the memory cache to save resources,
                 * then they will need to setup a custom HyperNodeServiceActivityMonitor if they still want to be able to know what's going on in the server. Custom
                 * HyperNodeServiceActivityMonitor objects can record activity to a database or some other data store, which the user can then query for the desired
                 * activity.
                 *****************************************************************************************************************/
                if (EnableTaskProgressCache && currentTaskInfo.Message.ProgressCacheRequested())
                {
                    Logger.LogTrace("Adding task progress cache monitor.");
                    systemActivityMonitors.Add(_taskProgressCacheMonitor);
                }

                /*****************************************************************************************************************
                 * If a task trace was requested, then we'll go ahead and honor it, but we will treat it differently depending
                 * on whether the task runs synchronously or asynchronously. If the task runs synchronously, then we'll return
                 * the task trace in the response as requested. However, if the task runs asynchronously, then the task trace will
                 * NOT be returned in the immediate response, but will be added to the response after the task completes. If the
                 * progress cache is enabled and was utilized as part of the request, then the response stored in the cache will
                 * include the task trace.
                 *****************************************************************************************************************/
                if (currentTaskInfo.Message.TaskTraceRequested())
                {
                    Logger.LogTrace("Adding task trace monitor.");
                    systemActivityMonitors.Add(taskTraceMonitor);
                }

                /*****************************************************************************************************************
                 * Subscribe our system activity monitors to the event stream first
                 *****************************************************************************************************************/
                Logger.LogTrace("Subscribing built-in activity monitors to activity tracker (Count: {c}).", systemActivityMonitors.Count);
                currentTaskInfo.ActivityObservers.AddRange(
                    systemActivityMonitors.Select(
                        monitor => new HyperNodeActivityObserver(
                            monitor,
                            liveEvents,
                            Scheduler.CurrentThread,
                            currentTaskInfo
                        )
                    )
                );

                /*****************************************************************************************************************
                 * Subscribe our custom activity monitors to the event stream last, just in case any of them throw exceptions. If they do, we've already setup our
                 * task trace and cache monitors at this point, so we can actually use the activity tracker to track the exceptions. This way, we can make sure that
                 * any exceptions thrown by user code are available to be reported to the client.
                 *
                 * Also, see the following for discussions about the various schedulers:
                 *     http://stackoverflow.com/questions/25993482/creating-an-sta-thread-when-using-reactive-extensions-rx-schedulers
                 *         - Difference between NewThreadScheduler and EventLoopScheduler
                 *     http://stackoverflow.com/questions/26159965/rx-taskpoolscheduler-vs-eventloopscheduler-memory-usage
                 *         - Excessive memory usage from TaskPoolScheduler
                 * As far as these schedulers go, I have decided to force all activity monitors to go through the current thread. This is because I don't want to
                 * blow up the server with all kinds of extra threads. Right now, I feel that if the activity monitors are slowing down the server to the point
                 * that we have to introduce some concurrency, then the caller can just indicate in the request object to run the task concurrently, so that the
                 * extra time consumed by the activity monitors is absorbed into the more general task thread.
                 *****************************************************************************************************************/
                Logger.LogTrace("Subscribing custom activity monitors to activity tracker (Count: {c}).", _customActivityMonitors.Count);
                currentTaskInfo.ActivityObservers.AddRange(
                    _customActivityMonitors.Where(
                        m => m.Enabled // Only add the monitors that are enabled
                    ).Select(
                        monitor => new HyperNodeActivityObserver(
                            monitor,
                            liveEvents,
                            Scheduler.CurrentThread,
                            currentTaskInfo
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                // This is a safe thing to do, it may just result in nothing being reported if Reactive Extensions wasn't setup properly
                currentTaskInfo.Activity.TrackException(
                    new ActivityTrackerInitializationException(
                        "An exception was thrown while initializing the activity tracker. See inner exception for details.",
                        ex
                    )
                );
            }
        }

        private HyperNodeActionReasonType? GetRejectionReason(HyperNodeTaskInfo taskInfo, ITaskActivityTracker queueTracker)
        {
            HyperNodeActionReasonType? rejectionReason = null;
            
            HyperNodeActivityItem? userRejectionActivity = null;
            try
            {
                // Allow the user to reject the message if necessary
                EventHandler.OnMessageReceived(
                    new MessageReceivedEventArgs(
                        queueTracker,
                        taskInfo,
                        r =>
                        {
                            rejectionReason = HyperNodeActionReasonType.Custom;
                            userRejectionActivity = new HyperNodeActivityItem
                            {
                                Agent = HyperNodeName,
                                EventDescription = "The message was rejected by user-defined code. See the EventDetail property for the rejection reason.",
                                EventDetail = r
                            };
                        }
                    )
                );
            }
            catch (Exception ex)
            {
                // User-defined event handler threw an exception, so assume the worst and reject the message.
                rejectionReason = HyperNodeActionReasonType.Custom;
                userRejectionActivity = new HyperNodeActivityItem
                {
                    Agent = HyperNodeName,
                    EventDescription = "An exception was thrown by user-defined code while processing the OnMessageReceived event. See the EventDetail property for the exception details.",
                    EventDetail = ex.ToString()
                };
            }

            // If the user didn't reject the message, give the system a chance to reject it
            var systemRejectionActivity = new HyperNodeActivityItem
            {
                Agent = HyperNodeName
            };

            if (!rejectionReason.HasValue)
            {
                if (MaxConcurrentTasks > -1 && _liveTasks.Count >= MaxConcurrentTasks)
                {
                    rejectionReason = HyperNodeActionReasonType.MaxConcurrentTaskCountReached;
                    systemRejectionActivity.EventDescription = $"The maximum number of concurrent tasks ({MaxConcurrentTasks}) has been reached.";
                    systemRejectionActivity.EventDetail = "An attempted was made to start a new task when the maximum number of concurrent tasks were already running. Consider increasing the value of MaxConcurrentTasks or setting it to -1 (unlimited).";
                }
                else if (_masterTokenSource.IsCancellationRequested)
                {
                    rejectionReason = HyperNodeActionReasonType.CancellationRequested;
                    systemRejectionActivity.EventDescription = "The service is shutting down. No new tasks can be started.";
                    systemRejectionActivity.EventDetail = "The service-level cancellation token has been triggered. No new tasks are being spun up and all existing tasks are in the process of shutting down.";
                }
                else
                {
                    // If we get this far, there's a good chance the message will not be rejected. However, if the ITaskIdProvider is ill-behaved (throws an exception), then we
                    // may still have to reject the message due to being unable to get a task ID

                    string? taskId = null;
                    
                    try
                    {
                        // Try to use our custom task ID provider
                        taskId = TaskIdProvider.CreateTaskId(taskInfo.MessageInfo);

                        Logger.LogTrace("Generated TaskID: {tid}", taskId == null ? "<null>" : $"'{taskId}'");
                    }
                    catch (Exception ex)
                    {
                        // Failed to get a Task ID, so reject the message
                        rejectionReason = HyperNodeActionReasonType.TaskIdProviderThrewException;
                        systemRejectionActivity.EventDescription = "An exception was thrown while attempting to create a task ID.";
                        systemRejectionActivity.EventDetail = ex.ToString();
                    }

                    if (string.IsNullOrWhiteSpace(taskId))
                    {
                        rejectionReason = HyperNodeActionReasonType.InvalidTaskId;
                        systemRejectionActivity.EventDescription = $"The class '{TaskIdProvider.GetType().FullName}' created a blank task ID.";
                    }
                    else if (!_liveTasks.TryAdd(taskId, taskInfo))
                    {
                        rejectionReason = HyperNodeActionReasonType.DuplicateTaskId;
                        systemRejectionActivity.EventDescription = $"A task with ID '{taskId}' is already running.";
                        systemRejectionActivity.EventDetail = "A duplicate task ID was generated. This can occur when a new instance of singleton task is started while an existing instance of that task is running. If you know the task will complete, please try again after the task is finished. You may also consider cancelling the task and then restarting it.";
                    }
                    else
                    {
                        // In this case we don't want to reject the message after all
                        systemRejectionActivity = null;

                        // Don't forget to set the task ID in our task info object
                        taskInfo.TaskId = taskId;
                    }
                }
            }

            // Set the rejection activity here, and let the user-defined rejection take precedence over the system-defined rejection
            var effectiveRejectionActivity = userRejectionActivity ?? systemRejectionActivity;

            // Finally, queue up the rejection activity last, after all of the user's other events they may have tracked
            if (effectiveRejectionActivity != null)
                queueTracker.Track(effectiveRejectionActivity.EventDescription, effectiveRejectionActivity.EventDetail);

            return rejectionReason;
        }

        private async Task ProcessMessageInternal(HyperNodeTaskInfo args)
        {
            ICommandResponse commandResponse;

            if (!string.IsNullOrWhiteSpace(args.Message.CommandName) &&
                _commandModuleConfigurations.ContainsKey(args.Message.CommandName) &&
                _commandModuleConfigurations.TryGetValue(args.Message.CommandName, out var commandModuleConfig) &&
                commandModuleConfig.Enabled)
            {
                // Create our command module instance (with DI if available)
                var commandInstance = ServiceProvider != null
                    ? ActivatorUtilities.CreateInstance(ServiceProvider, commandModuleConfig.CommandModuleType)
                    : Activator.CreateInstance(commandModuleConfig.CommandModuleType);

                IContractSerializer? contractSerializer = null;

                // Use the factories to create serializers, if applicable
                if (commandInstance is IContractSerializerFactory contractSerializerFactory)
                    contractSerializer = contractSerializerFactory.Create();

                // Allow the command module factory-created serializers to take precedence over the configured serializers
                contractSerializer ??= commandModuleConfig.ContractSerializer ?? DefaultContractSerializer;

                ICommandRequest? commandRequest;
                try
                {
                    // Deserialize the request string
                    commandRequest = contractSerializer.DeserializeRequest(args.Message.CommandRequestBytes);
                }
                catch (Exception ex)
                {
                    throw new InvalidCommandRequestTypeException(
                        $"Command '{args.Message.CommandName}' expected a request type of '{contractSerializer.GetRequestType().FullName}', but the command request string could not be deserialized into that type. See inner exception for details.",
                        ex
                    );
                }

                var loggerFactory = ServiceProvider?.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
                var commandLogger = loggerFactory.CreateLogger(commandModuleConfig.CommandModuleType);

                TrackActivityEventHandler commandActivityHandler = (_, e) => commandLogger.LogTrace("{m}", e.ActivityItem.EventDescription);

                try
                {
                    // Create the execution context to pass into our module
                    var context = new CommandExecutionContext
                    {
                        TaskId = args.TaskId,
                        ExecutingNodeName = HyperNodeName,
                        CommandName = args.Message.CommandName,
                        CreatedByAgentName = args.Message.CreatedByAgentName,
                        ProcessOptionFlags = args.Message.ProcessOptionFlags,
                        Request = commandRequest,
                        Activity = args.Activity,
                        Logger = commandLogger,
                        Token = args.Token
                    };

                    // Subscribe our command activity handler to the activity feed
                    if (args.Activity != null)
                        args.Activity.TrackActivityHandler += commandActivityHandler;

                    // Execute the command
                    commandResponse = commandInstance switch
                    {
                        IAwaitableCommandModule awaitableCommand => await awaitableCommand.Execute(context).ConfigureAwait(false),
                        ICommandModule nonAwaitableCommand => nonAwaitableCommand.Execute(context),
                        _ => throw new InvalidOperationException($"Command instance does not implement {nameof(ICommandModule)} or {nameof(IAwaitableCommandModule)}.")
                    };

                    // Serialize the response to send back
                    args.Response.CommandResponseBytes = contractSerializer.SerializeResponse(commandResponse);
                }
                finally
                {
                    // Unsubscribe our command activity handler from any further updates
                    if (args.Activity != null)
                        args.Activity.TrackActivityHandler -= commandActivityHandler;

                    // Check if our module is disposable and take care of it appropriately
                    (commandInstance as IDisposable)?.Dispose();
                }
            }
            else
            {
                // Unrecognized command
                commandResponse = (
                    MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommand
                ).ToEmptyCommandResponse();

                args.Activity?.Track($"Invalid {nameof(args.Message.CommandName)} '{args.Message.CommandName}'.");
            }

            // Make sure we report cancellation if it was requested
            if (args.Token.IsCancellationRequested)
                commandResponse.ProcessStatusFlags |= MessageProcessStatusFlags.Cancelled;

            args.Response.ProcessStatusFlags = commandResponse.ProcessStatusFlags;
        }

        private void AddCommandModuleConfiguration(CommandModuleConfiguration commandConfig)
        {
            if (commandConfig == null)
                throw new ArgumentNullException(nameof(commandConfig));

            if (string.IsNullOrWhiteSpace(commandConfig.CommandName))
                throw new ArgumentException($"The {nameof(commandConfig.CommandName)} property of the {nameof(commandConfig)} parameter must not be null or whitespace.", nameof(commandConfig));

            if (!_commandModuleConfigurations.TryAdd(commandConfig.CommandName, commandConfig))
            {
                throw new DuplicateCommandException(
                    $"A command already exists with the {nameof(commandConfig.CommandName)} '{commandConfig.CommandName}'."
                );
            }
        }

        /// <summary>
        /// Removes the task with the specified <paramref name="taskId"/> from the internal dictionary of tasks and calls Dispose() on it.
        /// </summary>
        /// <param name="taskId">The ID of the task to clean up.</param>
        private void TaskCleanUp(string? taskId)
        {
            // Remove our task info and dispose of it
            if (taskId != null && _liveTasks.TryRemove(taskId, out var taskInfo))
                taskInfo.Dispose();
        }

        private IEnumerable<Task> GetChildTasks()
        {
            return _liveTasks.Keys.Select(
                taskId => _liveTasks.TryGetValue(taskId, out var task)
                    ? task.WhenChildTasks()
                    : Task.CompletedTask
            );
        }

        #endregion Private Methods
    }
}
