using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.Configuration;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class EchoCommand : ICommandModule
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not EchoRequest request)
                throw new InvalidCommandRequestTypeException(typeof(EchoRequest), context.Request.GetType());

            var echoString = $"HyperNode '{context.ExecutingNodeName}' says, \"{request.Prompt}\".";
            
            context.Activity.Track(echoString);

            return new EchoResponse
            {
                ProcessStatusFlags = MessageProcessStatusFlags.Success,
                Reply = echoString
            };
        }
    }
}
