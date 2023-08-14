using HyperSoa.Contracts;

namespace HyperSoa.Service.Serialization
{
    internal sealed class NoOpContractSerializer : IContractSerializer<ByteArrayRequest, ByteArrayResponse>
    {
        ICommandRequest IContractSerializer.DeserializeRequest(byte[]? requestBytes)
        {
            return DeserializeRequest(requestBytes);
        }

        public byte[]? SerializeResponse(ByteArrayResponse? response)
        {
            return response?.ResponseBytes;
        }

        public ByteArrayRequest DeserializeRequest(byte[]? requestBytes)
        {
            return new ByteArrayRequest
            {
                RequestBytes = requestBytes
            };
        }

        public byte[]? SerializeResponse(ICommandResponse? response)
        {
            return SerializeResponse(response as ByteArrayResponse);
        }
    }
}
