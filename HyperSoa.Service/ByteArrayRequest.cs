using HyperSoa.Contracts;
using ProtoBuf;

namespace HyperSoa.Service
{
    [ProtoContract]
    public sealed class ByteArrayRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public byte[]? RequestBytes { get; set; }
    }
}
