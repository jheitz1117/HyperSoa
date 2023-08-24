using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableActivityMonitorCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public EnableActivityMonitorCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableActivityMonitorRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableActivityMonitorRequest), context.Request?.GetType());

            var processStatusFlags = MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            if (_adminService.IsKnownActivityMonitor(request.ActivityMonitorName))
            {
                var result = _adminService.EnableActivityMonitor(request.ActivityMonitorName, request.Enable);
                context.Activity.Track(
                    $"The activity monitor '{request.ActivityMonitorName}' {(result ? "is now" : "could not be")} {(request.Enable ? "enabled" : "disabled")}."
                );

                processStatusFlags = (result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure);
            }
            else
            {
                context.Activity.Track($"No activity monitor exists with the name '{request.ActivityMonitorName}'.");
            }

            return processStatusFlags.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableActivityMonitorRequest, EmptyCommandResponse>();
        }
    }
}
