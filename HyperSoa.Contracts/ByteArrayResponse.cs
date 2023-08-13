using ProtoBuf;

namespace HyperSoa.Contracts
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
