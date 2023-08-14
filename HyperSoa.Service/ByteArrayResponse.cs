using HyperSoa.Contracts;
using ProtoBuf;

namespace HyperSoa.Service
{
    [ProtoContract]
    public sealed class ByteArrayResponse : ICommandResponse
    {
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
        
        [ProtoMember(2)]
        public byte[]? ResponseBytes { get; set; }
    }
}
