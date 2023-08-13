using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Extensions;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class CancelTaskCommand : ICommandModule
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
    }
}
