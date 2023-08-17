using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class CancelTaskCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not CancelTaskRequest request)
                throw new InvalidCommandRequestTypeException(typeof(CancelTaskRequest), context.Request.GetType());

            var result = HyperNodeService.Instance.CancelTask(request.TaskId);
            context.Activity.Track(
                $"The task with ID '{request.TaskId}' {(result ? "was" : "could not be")} cancelled."
            );

            return (
                result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure
            ).ToEmptyCommandResponse();
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<CancelTaskRequest, EmptyCommandResponse>();
        }
    }
}
