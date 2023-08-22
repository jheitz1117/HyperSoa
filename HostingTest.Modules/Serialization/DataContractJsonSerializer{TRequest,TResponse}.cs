using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using HyperSoa.Contracts;
using HyperSoa.Service.Serialization;

namespace HostingTest.Modules.Serialization
{
    public sealed class DataContractJsonSerializer<TRequest, TResponse> : IServiceContractSerializer<TRequest, TResponse>
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

        ICommandRequest? IServiceContractSerializer.DeserializeRequest(byte[]? requestBytes)
        {
            return DeserializeRequest(requestBytes);
        }

        public TRequest? DeserializeRequest(byte[]? requestBytes)
        {
            if (requestBytes == null)
                return default;

            using (var memory = new MemoryStream(requestBytes))
            {
                return (TRequest?)_requestSerializer.ReadObject(memory);
            }
        }

        public byte[]? SerializeResponse(TResponse? response)
        {
            return SerializeResponse(response as ICommandResponse);
        }

        public byte[]? SerializeResponse(ICommandResponse? response)
        {
            if (response == null)
                return null;

            using (var memory = new MemoryStream())
            {
                _responseSerializer.WriteObject(memory, response);
                memory.Flush();

                return memory.ToArray();
            }
        }

        public Type GetRequestType()
        {
            return typeof(TRequest);
        }
    }
}
