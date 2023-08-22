using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EchoCommand : ICommandModule, IServiceContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EchoRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EchoRequest), context.Request?.GetType());

            var echoString = $"HyperNode '{context.ExecutingNodeName}' says, \"{request.Prompt}\".";
            
            context.Activity.Track(echoString);

            return new EchoResponse
            {
                ProcessStatusFlags = MessageProcessStatusFlags.Success,
                Reply = echoString
            };
        }

        public IServiceContractSerializer Create()
        {
            return new ProtoContractSerializer<EchoRequest, EchoResponse>();
        }
    }
}
