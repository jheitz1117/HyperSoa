using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface ICommandMetaData<out T>
        where T : ICommandRequest
    {
        public T? CommandRequest { get; }
        public IClientContractSerializer? Serializer { get; set; }
        public string? CreatedByAgentName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
        public Action<HyperNodeMessageRequest, HyperNodeMessageResponse>? OnHyperNodeResponse { get; set; }
    }
}
