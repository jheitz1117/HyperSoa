using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HostingTest.Client.Serialization
{
    public sealed class DataContractJsonSerializer<TRequest, TResponse> : IClientContractSerializer<TRequest, TResponse>
        where TRequest : ICommandRequest
        where TResponse : ICommandResponse
    {
        private readonly XmlObjectSerializer _requestSerializer;
        private readonly XmlObjectSerializer _responseSerializer;

        public DataContractJsonSerializer()
        {
            var serializerSettings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss")
            };

            _requestSerializer = new DataContractJsonSerializer(
                typeof(TRequest),
                serializerSettings
            );

            _responseSerializer = new DataContractJsonSerializer(
                typeof(TResponse),
                serializerSettings
            );
        }

        ICommandResponse? IClientContractSerializer.DeserializeResponse(byte[]? responseBytes)
        {
            return DeserializeResponse(responseBytes);
        }

        public TResponse? DeserializeResponse(byte[]? responseBytes)
        {
            if (responseBytes == null)
                return default;

            using (var memory = new MemoryStream(responseBytes))
            {
                return (TResponse?)_responseSerializer.ReadObject(memory);
            }
        }

        public byte[]? SerializeRequest(TRequest? request)
        {
            return SerializeRequest(request as ICommandRequest);
        }

        public byte[]? SerializeRequest(ICommandRequest? request)
        {
            if (request == null)
                return null;

            using (var memory = new MemoryStream())
            {
                _requestSerializer.WriteObject(memory, request);
                memory.Flush();

                return memory.ToArray();
            }
        }
    }
}
