using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// The primary request object used by <see cref="IHyperNodeService"/> instances.
    /// </summary>
    [ProtoContract]
    public class HyperNodeMessageRequest
    {
        /// <summary>
        /// The name of the agent that created this <see cref="HyperNodeMessageRequest"/>.
        /// </summary>
        [ProtoMember(1)]
        public string CreatedByAgentName { get; set; }

        /// <summary>
        /// The name of the command to execute.
        /// </summary>
        [ProtoMember(2)]
        public string CommandName { get; set; }

        /// <summary>
        /// A string containing the request parameters for the command to execute.
        /// </summary>
        [ProtoMember(3)]
        public byte[] CommandRequestBytes { get; set; }

        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessOptionFlags"/> values indicating how this <see cref="HyperNodeMessageRequest"/> should be processed.
        /// </summary>
        [ProtoMember(4)]
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }
    }
}
