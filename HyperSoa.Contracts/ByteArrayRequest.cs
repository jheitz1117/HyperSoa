using ProtoBuf;

namespace HyperSoa.Contracts
{
    [ProtoContract]
    public sealed class ByteArrayRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public byte[]? RequestBytes { get; set; }
    }
}
