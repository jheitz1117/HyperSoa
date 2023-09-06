using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    [ProtoContract]
    public class GetNodeStatusRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public bool IncludeSelfTaskId { get; set; }
    }
}
