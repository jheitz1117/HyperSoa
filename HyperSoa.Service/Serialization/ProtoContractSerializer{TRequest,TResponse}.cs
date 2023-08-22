using HyperSoa.Contracts;

namespace HyperSoa.Service.Serialization
{
    public sealed class ProtoContractSerializer<TRequest, TResponse> : IServiceContractSerializer<TRequest, TResponse>
        where TRequest : ICommandRequest
        where TResponse : ICommandResponse
    {
        #region Public Methods

        ICommandRequest? IServiceContractSerializer.DeserializeRequest(byte[]? requestBytes)
        {
            return DeserializeRequest(requestBytes);
        }

        public TRequest? DeserializeRequest(byte[]? requestBytes)
        {
            return Deserialize<TRequest>(requestBytes);
        }

        public byte[]? SerializeResponse(TResponse? response)
        {
            return SerializeResponse(response as ICommandResponse);
        }

        public byte[]? SerializeResponse(ICommandResponse? response)
        {
            return Serialize(response);
        }

        public Type GetRequestType()
        {
            return typeof(TRequest);
        }

        #endregion Public Methods

        #region Private Methods

        private static byte[]? Serialize<T>(T instance)
        {
            if (instance == null)
                return null;

            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, instance);
                return ms.ToArray();
            }
        }

        private static T? Deserialize<T>(byte[]? data)
        {
            if (data == null)
                return default;

            using (var ms = new MemoryStream(data))
            {
                return ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }

        #endregion Private Methods
    }
}
