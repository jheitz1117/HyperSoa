using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// The primary response object used by <see cref="IHyperNodeService"/> instances.
    /// </summary>
    [ProtoContract]
    public class HyperNodeMessageResponse
    {
        /// <summary>
        /// The ID of the task started as a result of the <see cref="HyperNodeMessageRequest"/>. This value
        /// may be null or white space if no task could be started.
        /// </summary>
        [ProtoMember(1)]
        public string? TaskId { get; set; }

        /// <summary>
        /// The name of the <see cref="IHyperNodeService"/> that sent this <see cref="HyperNodeMessageResponse"/>.
        /// </summary>
        [ProtoMember(2)]
        public string? RespondingNodeName { get; set; }

        /// <summary>
        /// If the task completed, contains the total run time of the task.
        /// </summary>
        [ProtoMember(3)]
        public TimeSpan? TotalRunTime { get; set; }

        /// <summary>
        /// The <see cref="HyperNodeActionType"/> taken by the <see cref="IHyperNodeService"/> in response to the <see cref="HyperNodeMessageRequest"/>.
        /// </summary>
        [ProtoMember(4)]
        public HyperNodeActionType NodeAction { get; set; }

        /// <summary>
        /// Indicates why the <see cref="IHyperNodeService"/> chose to take the <see cref="HyperNodeActionType"/> reported in the <see cref="NodeAction"/> property.
        /// </summary>
        [ProtoMember(5)]
        public HyperNodeActionReasonType NodeActionReason { get; set; }

        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        [ProtoMember(6)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }

        /// <summary>
        /// If the <see cref="MessageProcessOptionFlags.ReturnTaskTrace"/> option flag was specified in the <see cref="HyperNodeMessageRequest"/>, contains a list of
        /// <see cref="HyperNodeActivityItem"/> objects tracing the progress of the task up until the point at which this <see cref="HyperNodeMessageResponse"/> was
        /// returned. If the <see cref="MessageProcessOptionFlags.RunConcurrently"/> option flag was specified in the <see cref="HyperNodeMessageRequest"/>, the task
        /// trace will likely be incomplete because the main thread could have completed before the task was finished.
        /// </summary>
        [ProtoMember(7)]
        public HyperNodeActivityItem[]? TaskTrace { get; set; }

        /// <summary>
        /// Contains the serialized response bytes from the command that was executed.
        /// </summary>
        [ProtoMember(8)]
        public byte[]? CommandResponseBytes { get; set; }
    }
}
