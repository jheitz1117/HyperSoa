using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class GetNodeStatusCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public GetNodeStatusCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            context.Activity.Track("Retrieving HyperNode status info.");

            return new GetNodeStatusResponse
            {
                DiagnosticsEnabled = _adminService.EnableDiagnostics,
                TaskProgressCacheDuration = HyperNodeService.TaskProgressCacheDuration,
                TaskProgressCacheEnabled = _adminService.EnableTaskProgressCache,
                MaxConcurrentTasks = _adminService.MaxConcurrentTasks,
                Commands = _adminService.GetCommandStatuses().ToArray(),
                ActivityMonitors = _adminService.GetActivityMonitorStatuses().ToArray(),
                LiveTasks = _adminService.GetLiveTaskStatuses().ToArray(),
                ProcessStatusFlags = MessageProcessStatusFlags.Success
            };
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EmptyCommandRequest, GetNodeStatusResponse>();
        }
    }
}
