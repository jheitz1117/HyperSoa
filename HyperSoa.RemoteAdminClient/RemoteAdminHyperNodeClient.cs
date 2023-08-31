using HyperSoa.Client;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;

namespace HyperSoa.RemoteAdminClient
{
    public class RemoteAdminHyperNodeClient : OpinionatedHyperNodeClientBase, IRemoteAdminHyperNodeClient
    {
        public RemoteAdminHyperNodeClient(IHyperNodeService underlyingService)
            : base(underlyingService)
        {
            if (underlyingService is IOpinionatedHyperNodeClient opinionatedClient)
            {
                ClientApplicationName = opinionatedClient.ClientApplicationName;
                ReturnTaskTrace = opinionatedClient.ReturnTaskTrace;
                CacheTaskProgress = opinionatedClient.CacheTaskProgress;
            }
        }

        public RemoteAdminHyperNodeClient(string endpoint, string clientApplicationName)
            : this(new HyperNodeHttpClient(endpoint))
        {
            ClientApplicationName = clientApplicationName;
        }

        #region Public Methods

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(GetCachedTaskProgressInfoRequest request)
        {
            return await ProcessMessageAsync<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>(
                RemoteAdminCommandName.GetCachedTaskProgressInfo,
                request
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync()
        {
            return await ProcessMessageAsync<EmptyCommandRequest, GetNodeStatusResponse>(
                RemoteAdminCommandName.GetNodeStatus,
                new EmptyCommandRequest()
            ).ConfigureAwait(false);
        }

        public async Task<EchoResponse> EchoAsync(EchoRequest request)
        {
            return await ProcessMessageAsync<EchoRequest, EchoResponse>(
                RemoteAdminCommandName.Echo,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableCommandAsync(EnableCommandModuleRequest request)
        {
            return await ProcessMessageAsync<EnableCommandModuleRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableCommand,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableActivityMonitorAsync(EnableActivityMonitorRequest request)
        {
            return await ProcessMessageAsync<EnableActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableActivityMonitor,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> RenameActivityMonitorAsync(RenameActivityMonitorRequest request)
        {
            return await ProcessMessageAsync<RenameActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.RenameActivityMonitor,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(EnableTaskProgressCacheRequest request)
        {
            return await ProcessMessageAsync<EnableTaskProgressCacheRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableTaskProgressCache,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> CancelTaskAsync(CancelTaskRequest request)
        {
            return await ProcessMessageAsync<CancelTaskRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.CancelTask,
                request
            ).ConfigureAwait(false);
        }

        public async Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(SetTaskProgressCacheDurationRequest request)
        {
            return await ProcessMessageAsync<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>(
                RemoteAdminCommandName.SetTaskProgressCacheDuration,
                request
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<TResponse> ProcessMessageAsync<TRequest, TResponse>(string commandName, TRequest request)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            // Create our message request
            var serializer = new ProtoContractSerializer<TRequest, TResponse>();

            TResponse? commandResponse = default;

            await ProcessMessageAsync(
                commandName,
                serializer.SerializeRequest(request),
                commandResponseBytes =>
                {
                    if (commandResponseBytes?.Length > 0)
                        commandResponse = serializer.DeserializeResponse(commandResponseBytes);
                }
            ).ConfigureAwait(false);

            return commandResponse ?? throw new InvalidOperationException("Unable to deserialize command response.");
        }
        
        #endregion Private Methods
    }
}
