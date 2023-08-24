using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    [ProtoContract]
    public sealed class EchoResponse : ICommandResponse
    {
        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }

        [ProtoMember(2)]
        public string? Reply { get; set; }
    }
}
