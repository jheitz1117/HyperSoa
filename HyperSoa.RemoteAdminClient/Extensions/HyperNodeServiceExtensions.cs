using HyperSoa.Client;
using HyperSoa.Contracts;

namespace HyperSoa.RemoteAdminClient.Extensions
{
    public static class HyperNodeServiceExtensions
    {
        public static IRemoteAdminHyperNodeClient AsRemoteAdminClient(this IHyperNodeService service, string? clientApplicationName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (service is IRemoteAdminHyperNodeClient remoteAdminClient)
                return remoteAdminClient;

            if (service is IOpinionatedHyperNodeClient opinionatedTarget)
            {
                opinionatedTarget.ClientApplicationName = clientApplicationName;
                
                return opinionatedTarget.AsRemoteAdminClient();
            }

            return new RemoteAdminHyperNodeClient(service)
            {
                ClientApplicationName = clientApplicationName
            };
        }

        public static IRemoteAdminHyperNodeClient AsRemoteAdminClient(this IOpinionatedHyperNodeClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (client is IRemoteAdminHyperNodeClient remoteAdminClient)
                return remoteAdminClient;

            return new RemoteAdminHyperNodeClient(client)
            {
                ClientApplicationName = client.ClientApplicationName
            };
        }
    }
}
