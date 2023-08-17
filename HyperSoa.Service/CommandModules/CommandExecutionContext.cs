using HyperSoa.Contracts;
using HyperSoa.Service.ActivityTracking;
using Microsoft.Extensions.Logging;

namespace HyperSoa.Service.CommandModules
{
    internal class CommandExecutionContext : ICommandExecutionContext
    {
        public string TaskId { get; set; }
        public string ExecutingNodeName { get; set; }
        public string CommandName { get; set; }
        public string? CreatedByAgentName { get; set; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }
        public ICommandRequest? Request { get; set; }
        public ITaskActivityTracker Activity { get; set; }
        public ILogger Logger { get; set; }
        public CancellationToken Token { get; set; }
    }
}
