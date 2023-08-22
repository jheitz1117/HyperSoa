using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableDiagnosticsCommand : ICommandModule, IServiceContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableDiagnosticsRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableDiagnosticsRequest), context.Request?.GetType());

            HyperNodeService.Instance.EnableDiagnostics = request.Enable;
            context.Activity.Track($"Diagnostics are now {(request.Enable ? "enabled" : "disabled")}.");

            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableDiagnosticsRequest, EmptyCommandResponse>();
        }
    }
}
