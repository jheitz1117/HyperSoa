using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HyperSoa.Client.Extensions
{
    public static class CommandRequestExtensions
    {
        public static HyperNodeMessageRequest ToHyperNodeMessageRequest<T>(this ICommandMetaData<T>? metaData, string commandName, bool runAsync)
            where T : ICommandRequest
        {
            var optionFlags = MessageProcessOptionFlags.None;
            if (metaData?.ReturnTaskTrace ?? false)
                optionFlags |= MessageProcessOptionFlags.ReturnTaskTrace;
            if (metaData?.CacheTaskProgress ?? false)
                optionFlags |= MessageProcessOptionFlags.CacheTaskProgress;
            if (runAsync)
                optionFlags |= MessageProcessOptionFlags.RunConcurrently;

            byte[]? commandRequestBytes = null;
            if (metaData?.Serializer != null)
                commandRequestBytes = metaData.Serializer.SerializeRequest(metaData.CommandRequest);

            return new HyperNodeMessageRequest
            {
                CommandName = commandName,
                CommandRequestBytes = commandRequestBytes,
                CreatedByAgentName = metaData?.CreatedByAgentName,
                ProcessOptionFlags = optionFlags
            };
        }

        public static ICommandMetaData<T> CreatedBy<T>(this T request, string? createdByAgentName)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData<T>(
                request
            ).CreatedBy(
                createdByAgentName
            );
        }

        public static ICommandMetaData<T> WithTaskTrace<T>(this T request, bool returnTaskTrace)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData<T>(
                request
            ).WithTaskTrace(
                returnTaskTrace
            );
        }

        public static ICommandMetaData<T> WithProgressCaching<T>(this T request, bool cacheTaskProgress)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData<T>(
                request
            ).WithProgressCaching(
                cacheTaskProgress
            );
        }

        public static ICommandMetaData<T> WithMetaData<T>(this T request)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.WithMetaData(null, false, false);
        }

        public static ICommandMetaData<T> WithMetaData<T>(this T request, string? createdByAgentName, bool returnTaskTrace, bool cacheTaskProgress)
            where T : ICommandRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new CommandMetaData<T>(request)
            {
                CreatedByAgentName = createdByAgentName,
                ReturnTaskTrace = returnTaskTrace,
                CacheTaskProgress = cacheTaskProgress
            };
        }

        public static ICommandMetaData<T> CreatedBy<T>(this ICommandMetaData<T> metaData, string? createdByAgentName)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.CreatedByAgentName = createdByAgentName;

            return metaData;
        }

        public static ICommandMetaData<T> WithTaskTrace<T>(this ICommandMetaData<T> metaData)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            return metaData.WithTaskTrace(true);
        }

        public static ICommandMetaData<T> WithTaskTrace<T>(this ICommandMetaData<T> metaData, bool returnTaskTrace)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.ReturnTaskTrace = returnTaskTrace;

            return metaData;
        }

        public static ICommandMetaData<T> WithProgressCaching<T>(this ICommandMetaData<T> metaData)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            return metaData.WithProgressCaching(true);
        }

        public static ICommandMetaData<T> WithProgressCaching<T>(this ICommandMetaData<T> metaData, bool cacheTaskProgress)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.CacheTaskProgress = cacheTaskProgress;

            return metaData;
        }

        public static ICommandMetaData<T> WithSerializer<T>(this ICommandMetaData<T> metaData, IClientContractSerializer? serializer)
            where T : ICommandRequest
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            metaData.Serializer = serializer;

            return metaData;
        }
    }
}
