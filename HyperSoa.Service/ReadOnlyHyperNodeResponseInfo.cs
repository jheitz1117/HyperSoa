using System.Collections.ObjectModel;
using HyperSoa.Contracts;
using HyperSoa.Service.Extensions;

namespace HyperSoa.Service
{
    /// <summary>
    /// This class provides a read-only version of a <see cref="HyperNodeMessageResponse"/> for use in user-defined code.
    /// </summary>
    internal class ReadOnlyHyperNodeResponseInfo : IReadOnlyHyperNodeResponseInfo
    {
        public string TaskId { get; }
        public string RespondingNodeName { get; }
        public TimeSpan? TotalRunTime { get; }
        public HyperNodeActionType NodeAction { get; }
        public HyperNodeActionReasonType NodeActionReason { get; }
        public MessageProcessStatusFlags ProcessStatusFlags { get; }
        public IReadOnlyList<HyperNodeActivityItem> TaskTrace { get; }
        public byte[]? CommandResponseBytes { get; }

        public ReadOnlyHyperNodeResponseInfo(HyperNodeMessageResponse response)
        {
            TaskId = response.TaskId;
            RespondingNodeName = response.RespondingNodeName;
            TotalRunTime = response.TotalRunTime;
            NodeAction = response.NodeAction;
            NodeActionReason = response.NodeActionReason;
            ProcessStatusFlags = response.ProcessStatusFlags;
            CommandResponseBytes = response.CommandResponseBytes;
            TaskTrace = new ReadOnlyCollection<HyperNodeActivityItem>(
                response.TaskTrace?.Select(
                    t => t.Clone() // Create deep copies so user-defined code can't modify the original objects
                ).ToArray() ?? Array.Empty<HyperNodeActivityItem>()
            );
        }
    }
}
