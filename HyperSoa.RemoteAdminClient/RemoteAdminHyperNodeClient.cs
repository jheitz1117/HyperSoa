using HyperSoa.Client;
using HyperSoa.Client.Extensions;
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
                ClientApplicationName = opinionatedClient.ClientApplicationName;
        }

        public RemoteAdminHyperNodeClient(string clientApplicationName, string endpoint)
            : this(new HyperNodeHttpClient(endpoint))
        {
            ClientApplicationName = clientApplicationName;
        }

        #region Public Methods

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(GetCachedTaskProgressInfoRequest request)
        {
            return await GetCachedTaskProgressInfoAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(ICommandMetaData<GetCachedTaskProgressInfoRequest> request)
        {
            return await ProcessMessageAsync<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>(
                RemoteAdminCommandName.GetCachedTaskProgressInfo,
                request
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync()
        {
            return await GetNodeStatusAsync(
                new EmptyCommandRequest().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(ICommandMetaData<EmptyCommandRequest> request)
        {
            return await ProcessMessageAsync<EmptyCommandRequest, GetNodeStatusResponse>(
                RemoteAdminCommandName.GetNodeStatus,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EchoResponse> EchoAsync(EchoRequest request)
        {
            return await EchoAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EchoResponse> EchoAsync(ICommandMetaData<EchoRequest> request)
        {
            return await ProcessMessageAsync<EchoRequest, EchoResponse>(
                RemoteAdminCommandName.Echo,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableCommandAsync(EnableCommandModuleRequest request)
        {
            return await EnableCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableCommandAsync(ICommandMetaData<EnableCommandModuleRequest> request)
        {
            return await ProcessMessageAsync<EnableCommandModuleRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableCommand,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableActivityMonitorAsync(EnableActivityMonitorRequest request)
        {
            return await EnableActivityMonitorAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableActivityMonitorAsync(ICommandMetaData<EnableActivityMonitorRequest> request)
        {
            return await ProcessMessageAsync<EnableActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableActivityMonitor,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> RenameActivityMonitorAsync(RenameActivityMonitorRequest request)
        {
            return await RenameActivityMonitorAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> RenameActivityMonitorAsync(ICommandMetaData<RenameActivityMonitorRequest> request)
        {
            return await ProcessMessageAsync<RenameActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.RenameActivityMonitor,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(EnableTaskProgressCacheRequest request)
        {
            return await EnableTaskProgressCacheAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(ICommandMetaData<EnableTaskProgressCacheRequest> request)
        {
            return await ProcessMessageAsync<EnableTaskProgressCacheRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableTaskProgressCache,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> CancelTaskAsync(CancelTaskRequest request)
        {
            return await CancelTaskAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> CancelTaskAsync(ICommandMetaData<CancelTaskRequest> request)
        {
            return await ProcessMessageAsync<CancelTaskRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.CancelTask,
                request
            ).ConfigureAwait(false);
        }

        public async Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(SetTaskProgressCacheDurationRequest request)
        {
            return await SetTaskProgressCacheDurationAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(ICommandMetaData<SetTaskProgressCacheDurationRequest> request)
        {
            return await ProcessMessageAsync<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>(
                RemoteAdminCommandName.SetTaskProgressCacheDuration,
                request
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<TResponse> ProcessMessageAsync<TRequest, TResponse>(string commandName, ICommandMetaData<TRequest> metaData)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            // Create our message request
            var serializer = new ProtoContractSerializer<TRequest, TResponse>();

            TResponse? commandResponse = default;

            await ProcessMessageAsync(
                commandName,
                metaData.WithSerializer(serializer),
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
