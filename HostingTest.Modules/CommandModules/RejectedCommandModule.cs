using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;
using HyperSoa.Contracts.Extensions;

namespace HostingTest.Modules.CommandModules
{
    public class RejectedCommandModule: ICommandModule
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            context.Activity.Track("Can't get here because the event handler rejects all requests for this command.");

            return MessageProcessStatusFlags.Failure.ToEmptyCommandResponse();
        }
    }
}
