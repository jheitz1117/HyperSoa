using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client.Extensions
{
    public static class CommandRequestExtensions
    {
        public static ICommandMetaData WithMetaData<T>(this T request)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(request);
        }

        public static ICommandMetaData CreatedBy<T>(this T request, string? createdByAgentName)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(
                request
            ).CreatedBy(
                createdByAgentName
            );
        }

        public static ICommandMetaData CreatedBy(this ICommandMetaData metaData, string? createdByAgentName)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.CreatedByAgentName = createdByAgentName;

            return metaData;
        }

        public static ICommandMetaData WithTaskTrace<T>(this T request)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.WithTaskTrace(true);
        }

        public static ICommandMetaData WithTaskTrace<T>(this T request, bool returnTaskTrace)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(
                request
            ).WithTaskTrace(
                returnTaskTrace
            );
        }

        public static ICommandMetaData WithTaskTrace(this ICommandMetaData metaData)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            return metaData.WithTaskTrace(true);
        }

        public static ICommandMetaData WithTaskTrace(this ICommandMetaData metaData, bool returnTaskTrace)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.ReturnTaskTrace = returnTaskTrace;

            return metaData;
        }

        public static ICommandMetaData WithProgressCaching<T>(this T request)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.WithProgressCaching(true);
        }

        public static ICommandMetaData WithProgressCaching<T>(this T request, bool cacheTaskProgress)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(
                request
            ).WithProgressCaching(
                cacheTaskProgress
            );
        }

        public static ICommandMetaData WithProgressCaching(this ICommandMetaData metaData)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            return metaData.WithProgressCaching(true);
        }

        public static ICommandMetaData WithProgressCaching(this ICommandMetaData metaData, bool cacheTaskProgress)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.CacheTaskProgress = cacheTaskProgress;

            return metaData;
        }

        public static ICommandMetaData WithSerializer<T>(this T request, IClientContractSerializer? serializer)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(
                request
            ).WithSerializer(
                serializer
            );
        }

        public static ICommandMetaData WithSerializer(this ICommandMetaData metaData, IClientContractSerializer? serializer)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.Serializer = serializer;

            return metaData;
        }

        public static ICommandMetaData WithResponseHandler<T>(this T request, HyperNodeResponseHandler? responseHandler)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData(
                request
            ).WithResponseHandler(
                responseHandler
            );
        }

        public static ICommandMetaData WithResponseHandler(this ICommandMetaData metaData, HyperNodeResponseHandler? responseHandler)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.ResponseHandler = responseHandler;

            return metaData;
        }
    }
}
