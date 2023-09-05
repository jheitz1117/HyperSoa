using HyperSoa.Client;
using HyperSoa.Client.Extensions;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HostingTest.Client.Extensions
{
    internal static class CommandRequestExtensions
    {
        public static ICommandMetaData WithDefaultSerializer<TRequest, TResponse>(this TRequest request)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.WithSerializer(
                new ProtoContractSerializer<TRequest, TResponse>()
            );
        }

        public static ICommandMetaData WithDefaultSerializer<TRequest, TResponse>(this ICommandMetaData metaData)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();

            return metaData;
        }
    }
}
