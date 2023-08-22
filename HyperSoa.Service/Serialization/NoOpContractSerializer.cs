using HyperSoa.Contracts;

namespace HyperSoa.Service.Serialization
{
    internal sealed class NoOpContractSerializer : IServiceContractSerializer<ByteArrayRequest, ByteArrayResponse>
    {
        ICommandRequest IServiceContractSerializer.DeserializeRequest(byte[]? requestBytes)
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

        public Type GetRequestType()
        {
            return typeof(ByteArrayRequest);
        }
    }
}
