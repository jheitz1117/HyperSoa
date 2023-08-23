using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class CancelTaskCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public CancelTaskCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not CancelTaskRequest request)
                throw new InvalidCommandRequestTypeException(typeof(CancelTaskRequest), context.Request?.GetType());

            var result = _adminService.CancelTask(request.TaskId);
            context.Activity.Track(
                $"The task with ID '{request.TaskId}' {(result ? "was" : "could not be")} cancelled."
            );

            return (
                result ? MessageProcessStatusFlags.Success : MessageProcessStatusFlags.Failure
            ).ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<CancelTaskRequest, EmptyCommandResponse>();
        }
    }
}
