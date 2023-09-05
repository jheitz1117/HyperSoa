using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface ICommandMetaData<out T> : ICommandMetaData
        where T : ICommandRequest
    {
        public T? CommandRequest { get; }
        public IClientContractSerializer? Serializer { get; set; }
    }
}
