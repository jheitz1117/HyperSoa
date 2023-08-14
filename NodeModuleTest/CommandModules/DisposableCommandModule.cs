using System.Diagnostics;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Service.CommandModules;

namespace NodeModuleTest.CommandModules
{
    public class DisposableCommandModule : ICommandModule, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            context.Activity.Track("Executing DisposableCommandModule.");
            
            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }

        public void Dispose()
        {
            Trace.WriteLine("Disposing of DisposableCommandModule.");
            IsDisposed = true;
        }
    }
}
