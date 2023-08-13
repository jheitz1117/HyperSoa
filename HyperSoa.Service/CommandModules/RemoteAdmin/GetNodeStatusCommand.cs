﻿using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class GetNodeStatusCommand : ICommandModule, IContractSerializerFactory
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

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<ICommandRequest, GetNodeStatusResponse>();
        }
    }
}
