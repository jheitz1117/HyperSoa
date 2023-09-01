using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts;

namespace HyperSoa.Client.RemoteAdmin
{
    public interface IRemoteAdminHyperNodeClient : IHyperNodeService
    {
        public Task<GetCachedTaskProgressInfoResponse> GetCachedTaskProgressInfoAsync(GetCachedTaskProgressInfoRequest request);
        public Task<GetNodeStatusResponse> GetNodeStatusAsync();
        public Task<EchoResponse> EchoAsync(EchoRequest request);
        public Task<EmptyCommandResponse> EnableCommandAsync(EnableCommandModuleRequest request);
        public Task<EmptyCommandResponse> EnableActivityMonitorAsync(EnableActivityMonitorRequest request);
        public Task<EmptyCommandResponse> RenameActivityMonitorAsync(RenameActivityMonitorRequest request);
        public Task<EmptyCommandResponse> EnableTaskProgressCacheAsync(EnableTaskProgressCacheRequest request);
        public Task<EmptyCommandResponse> EnableDiagnosticsAsync(EnableDiagnosticsRequest request);
        public Task<EmptyCommandResponse> CancelTaskAsync(CancelTaskRequest request);
        public Task<SetTaskProgressCacheDurationResponse> SetTaskProgressCacheDurationAsync(SetTaskProgressCacheDurationRequest request);
    }
}
