using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin.Models
{
    /// <summary>
    /// Describes the status of a task.
    /// </summary>
    [ProtoContract]
    public class LiveTaskStatus
    {
        /// <summary>
        /// The ID of the task.
        /// </summary>
        [ProtoMember(1)]
        public string? TaskId { get; set; }

        /// <summary>
        /// The name of the command which started the task.
        /// </summary>
        [ProtoMember(2)]
        public string? CommandName { get; set; }

        /// <summary>
        /// Indicates whether cancellation has been requested for the task.
        /// </summary>
        [ProtoMember(3)]
        public bool IsCancellationRequested { get; set; }

        /// <summary>
        /// Indicates how long the task has been running. This property is null unless diagnostics are turned on.
        /// </summary>
        [ProtoMember(4)]
        public TimeSpan? Elapsed { get; set; }
    }
}
