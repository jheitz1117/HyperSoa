using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class RenameActivityMonitorCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public RenameActivityMonitorCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not RenameActivityMonitorRequest request)
                throw new InvalidCommandRequestTypeException(typeof(RenameActivityMonitorRequest), context.Request?.GetType());

            var processStatusFlags = MessageProcessStatusFlags.None;

            if (string.IsNullOrWhiteSpace(request.OldName))
            {
                context.Activity.Track($"Invalid {nameof(request.OldName)} value. The value must not be blank.");
                processStatusFlags |= MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            }
            else if (!_adminService.IsKnownActivityMonitor(request.OldName))
            {
                context.Activity.Track($"No activity monitor exists with the name '{request.OldName}'.");
                processStatusFlags |= MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(request.NewName))
                {
                    var result = _adminService.RenameActivityMonitor(request.OldName, request.NewName);
                    context.Activity.Track($"The activity monitor '{request.OldName}' {(result ? "has been" : "could not be")} renamed to '{request.NewName}'.");

                    processStatusFlags = result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure;
                }
                else
                {
                    context.Activity.Track($"Invalid {nameof(request.NewName)} value. The value must not be blank.");
                    processStatusFlags |= MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
                }
            }

            return processStatusFlags.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<RenameActivityMonitorRequest, EmptyCommandResponse>();
        }
    }
}
