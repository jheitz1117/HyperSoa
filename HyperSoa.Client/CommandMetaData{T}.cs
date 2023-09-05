using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    internal class CommandMetaData<T> : ICommandMetaData<T>
        where T : ICommandRequest
    {
        public T? CommandRequest { get; }
        public IClientContractSerializer? Serializer { get; set; }
        public string? CreatedByAgentName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
        public HyperNodeResponseHandler? ResponseHandler { get; set; }

        public CommandMetaData()
        {
            CommandRequest = default;
        }

        public CommandMetaData(T? commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public byte[]? GetCommandRequestBytes()
        {
            return Serializer?.SerializeRequest(CommandRequest);
        }
    }
}
