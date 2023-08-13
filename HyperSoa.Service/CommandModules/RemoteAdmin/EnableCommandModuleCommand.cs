using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.Serialization;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Extensions;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableCommandModuleCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableCommandModuleRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableCommandModuleRequest), context.Request.GetType());

            var processStatusFlags = MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            if (request.CommandName == context.CommandName && !request.Enable)
            {
                context.Activity.Track(
                    $"The command '{request.CommandName}' cannot be disabled remotely. To disable this command, you must modify the configuration."
                );
            }
            else
            {
                if (HyperNodeService.Instance.IsKnownCommand(request.CommandName ?? ""))
                {
                    var result = HyperNodeService.Instance.EnableCommandModule(request.CommandName, request.Enable);
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

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableCommandModuleRequest, EmptyCommandResponse>();
        }
    }
}
