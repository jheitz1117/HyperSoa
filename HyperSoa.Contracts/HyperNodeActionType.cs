using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Actions that a <see cref="IHyperNodeService"/> can take when it receives a <see cref="HyperNodeMessageRequest"/>.
    /// </summary>
    [ProtoContract]
    public enum HyperNodeActionType
    {
        /// <summary>
        /// Indicates that the receiving <see cref="IHyperNodeService"/> was completely unable to recognize, forward, or otherwise process the message in any capacity.
        /// </summary>
        [ProtoEnum]
        None = 0,

        /// <summary>
        /// Indicates that the receiving <see cref="IHyperNodeService"/> accepted responsibility for processing the message.
        /// </summary>
        [ProtoEnum]
        Accepted = 1,

        /// <summary>
        /// Indicates that the receiving <see cref="IHyperNodeService"/> rejected the message because the message would have caused the service to enter an invalid state.
        /// </summary>
        [ProtoEnum]
        Rejected = 3
    }
}
