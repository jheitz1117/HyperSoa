using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EnableDiagnosticsCommand : ICommandModule, IServiceContractSerializerFactory
    {
        private readonly HyperNodeService _adminService;

        public EnableDiagnosticsCommand(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService adminService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _adminService = adminService;
        }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EnableDiagnosticsRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EnableDiagnosticsRequest), context.Request?.GetType());

            _adminService.EnableDiagnostics = request.Enable;
            context.Activity.Track($"Diagnostics are now {(request.Enable ? "enabled" : "disabled")}.");

            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EnableDiagnosticsRequest, EmptyCommandResponse>();
        }
    }
}
