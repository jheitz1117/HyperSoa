using HyperSoa.Client.Extensions;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
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

        protected virtual async Task<string> RunCommandAsync<TRequest>(string commandName, ICommandMetaData<TRequest>? metaData)
            where TRequest : ICommandRequest
        {
            string? taskId = null;

            await ProcessMessageAsync(
                commandName,
                metaData,
                true,
                (hyperNodeRequest, hyperNodeResponse) =>
                {
                    metaData?.OnHyperNodeResponse?.Invoke(hyperNodeRequest, hyperNodeResponse);

                    taskId = hyperNodeResponse.TaskId;
                }
            ).ConfigureAwait(false);

            return taskId ?? throw new InvalidOperationException("Unable to run task asynchronously.");
        }

        protected virtual async Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData<TRequest>? metaData)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            TResponse? commandResponse = default;

            await ProcessMessageAsync(
                commandName,
                metaData,
                false,
                (hyperNodeRequest, hyperNodeResponse) =>
                {
                    metaData?.OnHyperNodeResponse?.Invoke(hyperNodeRequest, hyperNodeResponse);

                    if (hyperNodeResponse.CommandResponseBytes?.Length > 0 && metaData?.Serializer != null)
                        commandResponse = (TResponse?)metaData.Serializer.DeserializeResponse(hyperNodeResponse.CommandResponseBytes);
                }
            ).ConfigureAwait(false);
            
            return commandResponse ?? throw new InvalidOperationException("Unable to deserialize command response.");
        }
        
        #endregion Protected Methods

        #region Private Methods

        private async Task ProcessMessageAsync<T>(string commandName, ICommandMetaData<T>? metaData, bool runAsync, Action<HyperNodeMessageRequest, HyperNodeMessageResponse>? onHyperNodeResponse = null)
            where T : ICommandRequest
        {
            var hyperNodeRequest = metaData.ToHyperNodeMessageRequest(
                commandName,
                runAsync
            );

            hyperNodeRequest.CreatedByAgentName ??= ClientApplicationName;

            var hyperNodeResponse = await ProcessMessageAsync(
                hyperNodeRequest
            ).ConfigureAwait(false);

            onHyperNodeResponse?.Invoke(hyperNodeRequest, hyperNodeResponse);

            // Error checking
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

        #endregion Private Methods
    }
}
