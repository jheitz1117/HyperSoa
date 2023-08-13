using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    [ProtoContract]
    public sealed class EchoRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public string Prompt { get; set; }
    }
}
