using HyperSoa.Client.Extensions;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public class HyperNodeResponseContext
    {
        public HyperNodeMessageRequest HyperNodeRequest { get; }
        public HyperNodeMessageResponse HyperNodeResponse { get; }
        public ICommandRequest? CommandRequest { get; internal set; }
        public ICommandResponse? CommandResponse { get; internal set; }
        public bool ResponseHandled { get; set; }

        public HyperNodeResponseContext(HyperNodeMessageRequest hyperNodeRequest, HyperNodeMessageResponse hyperNodeResponse)
        {
            HyperNodeRequest = hyperNodeRequest ?? throw new ArgumentNullException(nameof(hyperNodeRequest));
            HyperNodeResponse = hyperNodeResponse ?? throw new ArgumentNullException(nameof(hyperNodeResponse));
        }
    }

    public delegate void HyperNodeResponseHandler(HyperNodeResponseContext args);

    public abstract class OpinionatedHyperNodeClientBase : IOpinionatedHyperNodeClient
    {
        private readonly IHyperNodeService _underlyingService;

        public string? ClientApplicationName { get; set; }

        protected OpinionatedHyperNodeClientBase(IHyperNodeService underlyingService)
        {
            _underlyingService = underlyingService ?? throw new ArgumentNullException(nameof(underlyingService));
        }

        #region Public Methods
        
        public async Task<HyperNodeMessageResponse> ProcessMessageAsync(HyperNodeMessageRequest message)
        {
            return await _underlyingService.ProcessMessageAsync(message).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Starts the specified command on the remote server and returns the associated task ID without waiting for the command to complete.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="metaData">Metadata describing how to execute the command. May optionally contain command request data.</param>
        /// <returns></returns>
        protected virtual async Task<string> RunCommandAsync(string commandName, ICommandMetaData? metaData = null)
        {
            string? taskId = null;

            await ProcessMessageAsync(
                commandName,
                (metaData ?? new CommandMetaDataImpl()).AddResponseHandler(
                    context => taskId = context.HyperNodeResponse.TaskId
                ),
                true
            ).ConfigureAwait(false);

            return taskId ?? throw new InvalidOperationException("Unable to run task asynchronously.");
        }

        /// <summary>
        /// Executes the specified command on the remote server and blocks until the command to completes or a timeout is reached.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="metaData">Metadata describing how to execute the command. May optionally contain command request data.</param>
        /// <returns></returns>
        protected virtual async Task ExecuteCommandAsync(string commandName, ICommandMetaData? metaData = null)
        {
            await ProcessMessageAsync(
                commandName,
                metaData ?? new CommandMetaDataImpl(),
                false
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the specified command on the remote server and blocks until the command to completes or a timeout is reached. If
        /// a command response is returned, it is deserialized.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="metaData">Metadata describing how to execute the command. May optionally contain command request data.</param>
        /// <returns></returns>
        protected virtual async Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            TResponse? commandResponse = default;

            await ProcessMessageAsync(
                commandName,
                (metaData ?? new CommandMetaDataImpl()).AddResponseHandler(
                    context => commandResponse = (TResponse?)context.CommandResponse
                ),
                false
            ).ConfigureAwait(false);
            
            return commandResponse ?? throw new InvalidOperationException("Unable to deserialize command response.");
        }
        
        #endregion Protected Methods

        #region Private Methods

        private async Task ProcessMessageAsync(string commandName, ICommandMetaData metaData, bool runAsync)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            var optionFlags = MessageProcessOptionFlags.None;
            if (metaData.ReturnTaskTrace)
                optionFlags |= MessageProcessOptionFlags.ReturnTaskTrace;
            if (metaData.CacheTaskProgress)
                optionFlags |= MessageProcessOptionFlags.CacheTaskProgress;
            if (runAsync)
                optionFlags |= MessageProcessOptionFlags.RunConcurrently;

            byte[]? commandRequestBytes = null;
            if (metaData is { Serializer: not null, CommandRequest: not null })
                commandRequestBytes = metaData.Serializer.SerializeRequest(metaData.CommandRequest);

            var hyperNodeRequest = new HyperNodeMessageRequest
            {
                CommandName = commandName,
                CommandRequestBytes = commandRequestBytes,
                CreatedByAgentName = metaData.CreatedByAgentName ?? ClientApplicationName,
                ProcessOptionFlags = optionFlags
            };

            var hyperNodeResponse = await ProcessMessageAsync(
                hyperNodeRequest
            ).ConfigureAwait(false);

            ICommandResponse? commandResponse = null;
            if (hyperNodeResponse.CommandResponseBytes?.Length > 0 && metaData.Serializer != null)
                commandResponse = metaData.Serializer.DeserializeResponse(hyperNodeResponse.CommandResponseBytes);

            var responseContext = new HyperNodeResponseContext(
                hyperNodeRequest,
                hyperNodeResponse
            )
            {
                CommandRequest = metaData.CommandRequest,
                CommandResponse = commandResponse
            };

            metaData.ResponseHandler?.Invoke(responseContext);

            // Only perform error checking here if the response was not fully handled by the custom handler above
            if (!responseContext.ResponseHandled)
            {
                if (hyperNodeResponse.ProcessStatusFlags.HasFlag(MessageProcessStatusFlags.InvalidCommand))
                    throw new ArgumentException($"The node '{hyperNodeResponse.RespondingNodeName}' does not recognize the command name '{hyperNodeRequest.CommandName}'.", nameof(hyperNodeRequest.CommandName));
                if (hyperNodeResponse.ProcessStatusFlags.HasFlag(MessageProcessStatusFlags.Cancelled))
                {
                    string errorMessage;
                    if (hyperNodeResponse.NodeAction == HyperNodeActionType.Rejected)
                    {
                        if (hyperNodeResponse.NodeActionReason == HyperNodeActionReasonType.DuplicateTaskId)
                            throw new InvalidOperationException($"The node '{hyperNodeResponse.RespondingNodeName}' is already running the specified task.");

                        errorMessage = $"The node '{hyperNodeResponse.RespondingNodeName}' rejected the request. See the {nameof(hyperNodeResponse.NodeActionReason)} and {nameof(HyperNodeMessageResponse)}.{nameof(HyperNodeMessageResponse.TaskTrace)} properties for details.";
                    }
                    else
                    {
                        errorMessage = $"The request was cancelled before it could be completed by the node '{hyperNodeResponse.RespondingNodeName}'. See the {nameof(hyperNodeResponse.NodeActionReason)} property for details.";
                    }

                    throw new OperationCanceledException(errorMessage);
                }
                if (hyperNodeResponse.ProcessStatusFlags.HasFlag(MessageProcessStatusFlags.InvalidCommandRequest))
                {
                    throw new ArgumentException($"Either the request was the wrong type for the command '{hyperNodeRequest.CommandName}', or the request failed validation.", nameof(hyperNodeRequest.CommandRequestBytes));
                }
                if (hyperNodeResponse.ProcessStatusFlags.HasFlag(MessageProcessStatusFlags.Failure))
                {
                    var errorMessage = $"The node '{hyperNodeResponse.RespondingNodeName}' failed to process the request. ";

                    if (hyperNodeRequest.ProcessOptionFlags.HasFlag(MessageProcessOptionFlags.ReturnTaskTrace))
                        errorMessage += $"See the {nameof(HyperNodeMessageResponse)}.{nameof(HyperNodeMessageResponse.TaskTrace)} property for details.";
                    else
                        errorMessage += $"Set the {nameof(MessageProcessOptionFlags.ReturnTaskTrace)} option flag and send the request again to see the trace for details.";

                    throw new InvalidOperationException(errorMessage);
                }
            }
        }

        #endregion Private Methods
    }
}
