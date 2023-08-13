using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Describes an activity event reported by a <see cref="IHyperNodeService"/>.
    /// </summary>
    [ProtoContract]
    public class HyperNodeActivityItem
    {
        /// <summary>
        /// The date and time the event happened.
        /// </summary>
        [ProtoMember(1)]
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// The name of the agent reporting the activity event.
        /// </summary>
        [ProtoMember(2)]
        public string Agent { get; set; }

        /// <summary>
        /// The amount of time that has elapsed since the first <see cref="HyperNodeActivityItem"/> was tracked for the task.
        /// This value may be null unless diagnostics are enabled.
        /// </summary>
        [ProtoMember(3)]
        public TimeSpan? Elapsed { get; set; }

        /// <summary>
        /// A description of the activity event.
        /// </summary>
        [ProtoMember(4)]
        public string EventDescription { get; set; }

        /// <summary>
        /// A longer, more detailed description of the activity event.
        /// </summary>
        [ProtoMember(5)]
        public string EventDetail { get; set; }

        /// <summary>
        /// A numeric value representing the progress of a command module. This value may be used in conjunction
        /// with the <see cref="ProgressTotal"/> property to obtain a percentile.
        /// </summary>
        [ProtoMember(6)]
        public double? ProgressPart { get; set; }

        /// <summary>
        /// A numeric value representing the progress total of a command module. This value may be used in conjunction
        /// with the <see cref="ProgressPart"/> property to obtain a percentile.
        /// </summary>
        [ProtoMember(7)]
        public double? ProgressTotal { get; set; }
    }
}
