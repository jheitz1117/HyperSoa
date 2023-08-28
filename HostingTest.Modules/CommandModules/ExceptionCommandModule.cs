using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;

namespace HostingTest.Modules.CommandModules
{
    public class ExceptionCommandModule: ICommandModule
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            throw new Exception("Testing unhandled exception.");
        }
    }
}
