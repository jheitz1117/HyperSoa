using System.Diagnostics;
using HyperSoa.Service;
using HyperSoa.Service.TaskIdProviders;

namespace NodeModuleTest.TaskIdProviders
{
    public class NumericTaskIdProvider : TaskIdProviderBase, IDisposable
    {
        private static long _counter;
        private static readonly object Lock = new();

        public override string CreateTaskId(IReadOnlyHyperNodeMessageInfo message)
        {
            if (message.CommandName == "TestLongRunningCommand")
            {
                return "LadeeDa__TestLongRunningCommandKey";
            }
            else
            {
                lock (Lock)
                {
                    return _counter++.ToString();
                }
            }
        }

        public void Dispose()
        {
            Trace.WriteLine(
                $"Disposing of {GetType().FullName}"
            );
        }
    }
}
