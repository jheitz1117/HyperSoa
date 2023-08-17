using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class RenameActivityMonitorCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not RenameActivityMonitorRequest request)
                throw new InvalidCommandRequestTypeException(typeof(RenameActivityMonitorRequest), context.Request?.GetType());

            var processStatusFlags = MessageProcessStatusFlags.Failure | MessageProcessStatusFlags.InvalidCommandRequest;
            if (HyperNodeService.Instance.IsKnownActivityMonitor(request.OldName))
            {
                var result = HyperNodeService.Instance.RenameActivityMonitor(request.OldName, request.NewName);
                context.Activity.Track($"The activity monitor '{request.OldName}' {(result ? "has been" : "could not be")} renamed to '{request.NewName}'.");

                processStatusFlags = result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure;
            }
            else
            {
                context.Activity.Track($"No activity monitor exists with the name '{request.OldName}'.");
            }

            return processStatusFlags.ToEmptyCommandResponse();
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<RenameActivityMonitorRequest, EmptyCommandResponse>();
        }
    }
}
