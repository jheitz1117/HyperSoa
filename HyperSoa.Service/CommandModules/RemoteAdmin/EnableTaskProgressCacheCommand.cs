using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.Serialization;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Extensions;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableTaskProgressCacheCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableTaskProgressCacheRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableTaskProgressCacheRequest), context.Request.GetType());

            HyperNodeService.Instance.EnableTaskProgressCache = request.Enable;
            context.Activity.Track($"The task progress cache is now {(request.Enable ? "enabled" : "disabled")}.");

            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableTaskProgressCacheRequest, EmptyCommandResponse>();
        }
    }
}
