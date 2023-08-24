using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableTaskProgressCacheCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public EnableTaskProgressCacheCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableTaskProgressCacheRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableTaskProgressCacheRequest), context.Request?.GetType());

            _adminService.EnableTaskProgressCache = request.Enable;
            context.Activity.Track($"The task progress cache is now {(request.Enable ? "enabled" : "disabled")}.");

            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableTaskProgressCacheRequest, EmptyCommandResponse>();
        }
    }
}
