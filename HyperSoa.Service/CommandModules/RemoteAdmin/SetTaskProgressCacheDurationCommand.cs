using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class SetTaskProgressCacheDurationCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not SetTaskProgressCacheDurationRequest request)
                throw new InvalidCommandRequestTypeException(typeof(SetTaskProgressCacheDurationRequest), context.Request?.GetType());

            var response = new SetTaskProgressCacheDurationResponse
            {
                TaskProgressCacheEnabled = HyperNodeService.Instance.EnableTaskProgressCache
            };

            if (!response.TaskProgressCacheEnabled)
            {
                context.Activity.Track("Warning: The task progress cache is disabled.");
                response.ProcessStatusFlags |= MessageProcessStatusFlags.HadWarnings;
            }

            HyperNodeService.Instance.TaskProgressCacheDuration = request.CacheDuration;
            context.Activity.Track($"The task progress cache duration is now {request.CacheDuration}.");
            
            response.ProcessStatusFlags |= MessageProcessStatusFlags.Success;

            return response;
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<SetTaskProgressCacheDurationRequest, SetTaskProgressCacheDurationResponse>();
        }
    }
}
