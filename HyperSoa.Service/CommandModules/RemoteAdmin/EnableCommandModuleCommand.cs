using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableCommandModuleCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public EnableCommandModuleCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableCommandModuleRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableCommandModuleRequest), context.Request?.GetType());

            var processStatusFlags = MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            if (request.CommandName == context.CommandName && !request.Enable)
            {
                context.Activity.Track(
                    $"The command '{request.CommandName}' cannot be disabled remotely. To disable this command, you must modify the configuration."
                );
            }
            else
            {
                if (_adminService.IsKnownCommand(request.CommandName))
                {
                    var result = _adminService.EnableCommandModule(request.CommandName, request.Enable);
                    context.Activity.Track(
                        $"The command '{request.CommandName}' {(result ? "is now" : "could not be")} {(request.Enable ? "enabled" : "disabled")}."
                    );

                    processStatusFlags = (result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure);
                }
                else
                {
                    context.Activity.Track($"The command '{request.CommandName}' is invalid.");
                }
            }

            return processStatusFlags.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableCommandModuleRequest, EmptyCommandResponse>();
        }
    }
}
