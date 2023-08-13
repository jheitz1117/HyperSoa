using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin.Models
{
    /// <summary>
    /// Describes progress information for a task.
    /// </summary>
    [ProtoContract]
    public class HyperNodeTaskProgressInfo
    {
        /// <summary>
        /// A list of <see cref="HyperNodeActivityItem"/> objects tracing the progress of the task.
        /// </summary>
        [ProtoMember(1)]
        public List<HyperNodeActivityItem> Activity { get; set; } = new List<HyperNodeActivityItem>();

        /// <summary>
        /// If the task has completed, contains the <see cref="HyperNodeMessageResponse"/> of the task.
        /// </summary>
        [ProtoMember(2)]
        public HyperNodeMessageResponse Response { get; set; }

        /// <summary>
        /// Indicates whether the task has completed.
        /// </summary>
        [ProtoMember(4)]
        public bool IsComplete { get; set; }

        /// <summary>
        /// The latest partial progress value captured as of the time of the request.
        /// </summary>
        [ProtoMember(5)]
        public double? ProgressPart { get; set; }

        /// <summary>
        /// The latest total progress value captured as of the time of the request.
        /// </summary>
        [ProtoMember(6)]
        public double? ProgressTotal { get; set; }
    }
}
