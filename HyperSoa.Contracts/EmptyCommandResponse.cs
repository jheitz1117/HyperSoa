using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Wraps a bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
    /// This class is the default <see cref="ICommandResponse"/> type for all command modules if no other <see cref="ICommandResponse"/> is
    /// specified.
    /// </summary>
    [ProtoContract]
    public sealed class EmptyCommandResponse : ICommandResponse
    {
        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
    }
}
