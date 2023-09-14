using HyperSoa.Client.Extensions;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;

namespace HyperSoa.Client.RemoteAdmin
{
    public class RemoteAdminHyperNodeClient : OpinionatedHyperNodeClientBase, IRemoteAdminHyperNodeClient
    {
        public RemoteAdminHyperNodeClient(IHyperNodeService underlyingService)
            : base(underlyingService)
        {
            if (underlyingService is IOpinionatedHyperNodeClient opinionatedClient)
                ClientApplicationName = opinionatedClient.ClientApplicationName;
        }

        public RemoteAdminHyperNodeClient(string clientApplicationName, HttpClient httpClient, string endpoint)
            : this(new HyperNodeHttpClient(httpClient, endpoint))
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

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>(
                RemoteAdminCommandName.GetCachedTaskProgressInfo,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(GetNodeStatusRequest request)
        {
            return await GetNodeStatusAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<GetNodeStatusRequest, GetNodeStatusResponse>(
                RemoteAdminCommandName.GetNodeStatus,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EchoResponse> EchoAsync(EchoRequest request)
        {
            return await EchoAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EchoResponse> EchoAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<EchoRequest, EchoResponse>(
                RemoteAdminCommandName.Echo,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableCommandAsync(EnableCommandModuleRequest request)
        {
            return await EnableCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableCommandAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<EnableCommandModuleRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableCommand,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableActivityMonitorAsync(EnableActivityMonitorRequest request)
        {
            return await EnableActivityMonitorAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableActivityMonitorAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<EnableActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableActivityMonitor,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> RenameActivityMonitorAsync(RenameActivityMonitorRequest request)
        {
            return await RenameActivityMonitorAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> RenameActivityMonitorAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<RenameActivityMonitorRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.RenameActivityMonitor,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(EnableTaskProgressCacheRequest request)
        {
            return await EnableTaskProgressCacheAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<EnableTaskProgressCacheRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableTaskProgressCache,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableDiagnosticsAsync(EnableDiagnosticsRequest request)
        {
            return await EnableDiagnosticsAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableDiagnosticsAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<EnableDiagnosticsRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableDiagnostics,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> CancelTaskAsync(CancelTaskRequest request)
        {
            return await CancelTaskAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> CancelTaskAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<CancelTaskRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.CancelTask,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(SetTaskProgressCacheDurationRequest request)
        {
            return await SetTaskProgressCacheDurationAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>(
                RemoteAdminCommandName.SetTaskProgressCacheDuration,
                metaData
            ).ConfigureAwait(false);
        }

        public override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
        {
            // Always override the serializer since ours is the only valid one for remote admin commands
            return base.GetCommandResponseAsync<TRequest, TResponse>(
                commandName,
                metaData?.WithSerializer(
                    new ProtoContractSerializer<TRequest, TResponse>()
                )
            );
        }

        #endregion Public Methods
    }
}
