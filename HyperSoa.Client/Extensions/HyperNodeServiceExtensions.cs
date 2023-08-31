using HyperSoa.Contracts;

namespace HyperSoa.Client.Extensions
{
    public static class HyperNodeServiceExtensions
    {
        public static IOpinionatedHyperNodeClient From(this IHyperNodeService service, string? clientApplicationName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (service is IOpinionatedHyperNodeClient opinionatedClient)
            {
                opinionatedClient.ClientApplicationName = clientApplicationName;
                
                return opinionatedClient;
            }

            return new OpinionatedHyperNodeClient(service)
            {
                ClientApplicationName = clientApplicationName
            };
        }

        public static IOpinionatedHyperNodeClient WithTaskTrace(this IHyperNodeService service, string? clientApplicationName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return service.From(
                clientApplicationName
            ).WithTaskTrace();
        }

        public static IOpinionatedHyperNodeClient WithTaskTrace(this IHyperNodeService service, string? clientApplicationName, bool returnTaskTrace)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return service.From(
                clientApplicationName
            ).WithTaskTrace(returnTaskTrace);
        }

        public static IOpinionatedHyperNodeClient WithProgressCaching(this IHyperNodeService service, string? clientApplicationName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return service.From(
                clientApplicationName
            ).WithProgressCaching();
        }

        public static IOpinionatedHyperNodeClient WithProgressCaching(this IHyperNodeService service, string? clientApplicationName, bool cacheTaskProgress)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return service.From(
                clientApplicationName
            ).WithProgressCaching(cacheTaskProgress);
        }

        public static T WithTaskTrace<T>(this T client)
            where T : IOpinionatedHyperNodeClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return client.WithTaskTrace(true);
        }

        public static T WithTaskTrace<T>(this T client, bool returnTaskTrace)
            where T : IOpinionatedHyperNodeClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            client.ReturnTaskTrace = returnTaskTrace;

            return client;
        }

        public static T WithProgressCaching<T>(this T client)
            where T : IOpinionatedHyperNodeClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return client.WithProgressCaching(true);
        }

        public static T WithProgressCaching<T>(this T client, bool cacheTaskProgress)
            where T : IOpinionatedHyperNodeClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            client.CacheTaskProgress = cacheTaskProgress;

            return client;
        }
    }
}
