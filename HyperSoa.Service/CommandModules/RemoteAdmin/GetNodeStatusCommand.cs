using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class GetNodeStatusCommand : ICommandModule, IServiceContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            context.Activity.Track("Retrieving HyperNode status info.");

            return new GetNodeStatusResponse
            {
                DiagnosticsEnabled = HyperNodeService.Instance.EnableDiagnostics,
                TaskProgressCacheDuration = HyperNodeService.Instance.TaskProgressCacheDuration,
                TaskProgressCacheEnabled = HyperNodeService.Instance.EnableTaskProgressCache,
                MaxConcurrentTasks = HyperNodeService.Instance.MaxConcurrentTasks,
                Commands = HyperNodeService.Instance.GetCommandStatuses().ToList(),
                ActivityMonitors = HyperNodeService.Instance.GetActivityMonitorStatuses().ToList(),
                LiveTasks = HyperNodeService.Instance.GetLiveTaskStatuses().ToList(),
                ProcessStatusFlags = MessageProcessStatusFlags.Success
            };
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<ICommandRequest, GetNodeStatusResponse>();
        }
    }
}
