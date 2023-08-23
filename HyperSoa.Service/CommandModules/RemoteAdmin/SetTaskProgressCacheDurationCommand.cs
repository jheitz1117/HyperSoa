using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class SetTaskProgressCacheDurationCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public SetTaskProgressCacheDurationCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not SetTaskProgressCacheDurationRequest request)
                throw new InvalidCommandRequestTypeException(typeof(SetTaskProgressCacheDurationRequest), context.Request?.GetType());

            var response = new SetTaskProgressCacheDurationResponse
            {
                TaskProgressCacheEnabled = _adminService.EnableTaskProgressCache
            };

            if (!response.TaskProgressCacheEnabled)
            {
                context.Activity.Track("Warning: The task progress cache is disabled.");
                response.ProcessStatusFlags |= MessageProcessStatusFlags.HadWarnings;
            }

            _adminService.TaskProgressCacheDuration = request.CacheDuration;
            context.Activity.Track($"The task progress cache duration is now {request.CacheDuration}.");
            
            response.ProcessStatusFlags |= MessageProcessStatusFlags.Success;

            return response;
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>();
        }
    }
}
