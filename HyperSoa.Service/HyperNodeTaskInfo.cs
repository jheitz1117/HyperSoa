using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using HyperSoa.Contracts;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.ActivityTracking.Trackers;
using HyperSoa.Service.EventTracking;

namespace HyperSoa.Service
{
    internal sealed class HyperNodeTaskInfo : ITaskEventContext, IDisposable
    {
        private readonly Stopwatch? _stopwatch;
        private readonly CancellationTokenSource _taskTokenSource;
        private readonly List<Task> _childTasks = new();

        private IDisposable? _logScope;

        #region Properties

        public string ExecutingNodeName { get; }

        public string? CommandName => Message.CommandName;

        public IReadOnlyHyperNodeMessageInfo MessageInfo => new ReadOnlyHyperNodeMessageInfo(Message);

        public string? TaskId
        {
            get => Response.TaskId;
            set => Response.TaskId = value;
        }

        public CancellationToken Token => _taskTokenSource.Token;

        public HyperNodeTaskActivityTracker? Activity { get; set; }

        public IConnectableObservable<Unit> TerminatingSequence { get; } = Observable.Return(Unit.Default).Publish();

        public List<HyperNodeActivityObserver> ActivityObservers { get; } = new();

        public HyperNodeMessageRequest Message { get; }

        public HyperNodeMessageResponse Response { get; }

        public TimeSpan? Elapsed => _stopwatch?.Elapsed;

        #endregion Properties

        #region Public Methods

        public HyperNodeTaskInfo(string executingNodeName, HyperNodeMessageRequest message, HyperNodeMessageResponse response, bool enableDiagnostics, CancellationToken masterToken)
        {
            ExecutingNodeName = executingNodeName;
            _taskTokenSource = CancellationTokenSource.CreateLinkedTokenSource(masterToken);
            Message = message;
            Response = response;

            if (enableDiagnostics)
                _stopwatch = new Stopwatch();
        }

        public void StartStopwatch()
        {
            if (_stopwatch != null && !_stopwatch.IsRunning)
                _stopwatch.Start();
        }

        public void BeginTaskIdLogScope(IDisposable? logScope)
        {
            _logScope = logScope;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="List{T}"/>.
        /// </summary>
        /// <param name="childTask">The object to be added to the end of the <see cref="List{T}"/>. The value can be null.</param>
        public void AddChildTask(Task childTask)
        {
            _childTasks.Add(childTask);
        }

        /// <summary>
        /// Creates a task that will complete when all of the child tasks have completed.
        /// </summary>
        /// <returns></returns>
        public async Task WhenChildTasks()
        {
            await Task.WhenAll(_childTasks).ConfigureAwait(false);
        }

        /// <summary>
        /// Waits for all of the child <see cref="Task"/> objects to complete execution.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the tasks to complete.</param>
        /// <returns></returns>
        public void WaitChildTasks(CancellationToken token)
        {
            Task.WaitAll(_childTasks.ToArray(), token);
        }

        public void Cancel()
        {
            if (!_taskTokenSource.IsCancellationRequested)
                _taskTokenSource.Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // First, stop our stopwatch so we can record the total elapsed time for this task
                _stopwatch?.Stop();
                Response.TotalRunTime = _stopwatch?.Elapsed;

                /* Signal completion before we dispose our subscribers. This is necessary because clients who are polling the service for progress
                 * updates must know when the service is done sending updates. Make sure we pass the final, completed response object in case we have
                 * any monitors that are watching for it. */
                Activity?.TrackTaskComplete(Response);

                // Signal that we are done raising activity events to ensure that the queues for all of our schedulers don't keep having stuff appended to the end
                // This also triggers the OnComplete() event for all subscribers, which should automatically trigger the scheduling of their disposal
                TerminatingSequence.Connect().Dispose();

                /*
                 * If we dispose of our activity observers at this point, it's possible that some subscribers may be disposed before they've processed all of their queued items.
                 * Currently, the only way I can think of for the disposal to NOT happen is if someone writes an infinite loop into their observer to prevent the OnNext() from
                 * returning. If they've done this, I'm hoping the problem will be fairly obvious to the user. As it stands, I will not be manually disposing the activity
                 * subscribers at this time; I will allow the automatic disposal scheduling to take care of it. That is, when a sequence is completed, it calls Dispose(), and I'm
                 * guaranteeing that the event sequence will always complete.
                 */
                _taskTokenSource.Dispose();

                // Finally, dispose of our log scope
                _logScope?.Dispose();
            }
        }

        #endregion Private Methods
    }
}
