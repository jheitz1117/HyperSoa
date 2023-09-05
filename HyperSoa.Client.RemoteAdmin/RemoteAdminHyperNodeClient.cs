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

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>(
                RemoteAdminCommandName.GetCachedTaskProgressInfo,
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync()
        {
            return await GetNodeStatusAsync(false).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(bool excludeSelfTaskStatus)
        {
            return await GetNodeStatusAsync(
                new EmptyCommandRequest().CreatedBy(ClientApplicationName),
                excludeSelfTaskStatus
            ).ConfigureAwait(false);
        }

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(ICommandMetaData metaData, bool excludeSelfTaskStatus)
        {
            string? selfTaskId = null;

            // Needed to avoid infinite recursion in the closure below
            var innerHandler = metaData.ResponseHandler;

            var commandResponse = await GetCommandResponseAsync<EmptyCommandRequest, GetNodeStatusResponse>(
                RemoteAdminCommandName.GetNodeStatus,
                metaData.WithResponseHandler(
                    (hyperNodeRequest, hyperNodeResponse) =>
                    {
                        var responseHandled = innerHandler?.Invoke(
                            hyperNodeRequest,
                            hyperNodeResponse
                        ) ?? false;

                        selfTaskId = hyperNodeResponse.TaskId;

                        return responseHandled;
                    }
                )
            ).ConfigureAwait(false);

            if (excludeSelfTaskStatus)
            {
                commandResponse.LiveTasks = commandResponse.LiveTasks?.Where(
                    t => t.TaskId != selfTaskId
                ).ToArray();
            }

            return commandResponse;
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

        #endregion Public Methods

        #region Protected Methods

        protected override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
        {
            // Always override the serializer since ours is the only valid one for remote admin commands
            return base.GetCommandResponseAsync<TRequest, TResponse>(
                commandName,
                metaData?.WithSerializer(
                    new ProtoContractSerializer<TRequest, TResponse>()
                )
            );
        }

        #endregion Protected Methods
    }
}
