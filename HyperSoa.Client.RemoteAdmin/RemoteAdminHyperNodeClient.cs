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

        public async Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(ICommandMetaData<GetCachedTaskProgressInfoRequest> request)
        {
            return await GetCommandResponseAsync<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>(
                RemoteAdminCommandName.GetCachedTaskProgressInfo,
                request
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

        public async Task<GetNodeStatusResponse> GetNodeStatusAsync(ICommandMetaData<EmptyCommandRequest> request, bool excludeSelfTaskStatus)
        {
            string? selfTaskId = null;

            var commandResponse = await GetCommandResponseAsync<EmptyCommandRequest, GetNodeStatusResponse>(
                RemoteAdminCommandName.GetNodeStatus,
                request.RegisterSuccessDelegate(
                    (_, hyperNodeResponse) =>
                    {
                        selfTaskId = hyperNodeResponse.TaskId;
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

        public async Task<EchoResponse> EchoAsync(ICommandMetaData<EchoRequest> request)
        {
            return await GetCommandResponseAsync<EchoRequest, EchoResponse>(
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
            return await GetCommandResponseAsync<EnableCommandModuleRequest, EmptyCommandResponse>(
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
            return await GetCommandResponseAsync<EnableActivityMonitorRequest, EmptyCommandResponse>(
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
            return await GetCommandResponseAsync<RenameActivityMonitorRequest, EmptyCommandResponse>(
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
            return await GetCommandResponseAsync<EnableTaskProgressCacheRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableTaskProgressCache,
                request
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableDiagnosticsAsync(EnableDiagnosticsRequest request)
        {
            return await EnableDiagnosticsAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<EmptyCommandResponse> EnableDiagnosticsAsync(ICommandMetaData<EnableDiagnosticsRequest> request)
        {
            return await GetCommandResponseAsync<EnableDiagnosticsRequest, EmptyCommandResponse>(
                RemoteAdminCommandName.EnableDiagnostics,
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
            return await GetCommandResponseAsync<CancelTaskRequest, EmptyCommandResponse>(
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
            return await GetCommandResponseAsync<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>(
                RemoteAdminCommandName.SetTaskProgressCacheDuration,
                request
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData<TRequest>? metaData)
        {
            // Always override the serializer since ours is the only valid one for remote admin commands
            return base.GetCommandResponseAsync<TRequest, TResponse>(
                commandName,
                metaData?.WithSerializer(
                    new ProtoContractSerializer<TRequest, TResponse>()
                )
            );
        }

        protected override Task<string> RunCommandAsync<TRequest>(string commandName, ICommandMetaData<TRequest>? metaData)
        {
            // Always override the serializer since ours is the only valid one for remote admin commands
            return base.RunCommandAsync(
                commandName,
                metaData?.WithSerializer(
                    new ProtoContractSerializer<TRequest, ICommandResponse>()
                )
            );
        }

        #endregion Protected Methods
    }
}
