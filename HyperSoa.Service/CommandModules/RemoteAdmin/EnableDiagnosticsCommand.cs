using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.Serialization;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Extensions;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableDiagnosticsCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableDiagnosticsRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableDiagnosticsRequest), context.Request.GetType());

            HyperNodeService.Instance.EnableDiagnostics = request.Enable;
            context.Activity.Track($"Diagnostics are now {(request.Enable ? "enabled" : "disabled")}.");

            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableDiagnosticsRequest, EmptyCommandResponse>();
        }
    }
}
