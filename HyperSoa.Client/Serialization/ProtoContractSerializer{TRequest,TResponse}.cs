using HyperSoa.Contracts;

namespace HyperSoa.Client.Serialization
{
    public sealed class ProtoContractSerializer<TRequest, TResponse> : IContractSerializer<TRequest, TResponse>
        where TRequest : ICommandRequest
        where TResponse : ICommandResponse
    {
        #region Public Methods

        public byte[]? SerializeRequest(TRequest? request)
        {
            return SerializeRequest(request as ICommandRequest);
        }

        public byte[]? SerializeRequest(ICommandRequest? request)
        {
            return Serialize(request);
        }

        ICommandResponse? IContractSerializer.DeserializeResponse(byte[]? responseBytes)
        {
            return DeserializeResponse(responseBytes);
        }

        public TResponse? DeserializeResponse(byte[]? responseBytes)
        {
            return Deserialize<TResponse>(responseBytes);
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
