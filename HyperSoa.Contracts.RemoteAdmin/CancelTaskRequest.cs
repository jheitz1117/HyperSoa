using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    [ProtoContract]
    public sealed class CancelTaskRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public string TaskId { get; set; }
    }
}
