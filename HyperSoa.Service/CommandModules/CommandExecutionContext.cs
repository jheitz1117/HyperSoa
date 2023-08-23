using HyperSoa.Contracts;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.ActivityTracking.Trackers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HyperSoa.Service.CommandModules
{
    internal class CommandExecutionContext : ICommandExecutionContext
    {
        public string? TaskId { get; set; }
        public string? ExecutingNodeName { get; set; }
        public string? CommandName { get; set; }
        public string? CreatedByAgentName { get; set; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }
        public ICommandRequest? Request { get; set; }
        public ITaskActivityTracker Activity { get; set; } = NullActivityTracker.Instance;
        public ILogger Logger { get; set; } = NullLogger.Instance;
        public CancellationToken Token { get; set; }
    }
}
