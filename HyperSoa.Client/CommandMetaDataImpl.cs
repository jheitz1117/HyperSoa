using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    internal class CommandMetaDataImpl : ICommandMetaData
    {
        public string? CreatedByAgentName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
        public ICommandRequest? CommandRequest { get; }
        public IClientContractSerializer? Serializer { get; set; }
        public HyperNodeResponseHandler? ResponseHandler { get; set; }
     
        public CommandMetaDataImpl()
        {
            CommandRequest = default;
        }

        public CommandMetaDataImpl(ICommandRequest? commandRequest)
        {
            CommandRequest = commandRequest;
        }
    }
}
