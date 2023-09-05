using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface ICommandMetaData
    {
        public string? CreatedByAgentName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
        public ICommandRequest? CommandRequest { get; }
        public IClientContractSerializer? Serializer { get; set; }
        public HyperNodeResponseHandler? ResponseHandler { get; set; }
    }
}
