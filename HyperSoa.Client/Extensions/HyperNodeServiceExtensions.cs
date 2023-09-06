using HyperSoa.Contracts;

namespace HyperSoa.Client.Extensions
{
    public static class HyperNodeServiceExtensions
    {
        public static IOpinionatedHyperNodeClient AsOpinionatedClient(this IHyperNodeService service, string? clientApplicationName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (service is IOpinionatedHyperNodeClient opinionatedTarget)
            {
                opinionatedTarget.ClientApplicationName = clientApplicationName;
                
                return opinionatedTarget;
            }

            return new OpinionatedHyperNodeClient(service)
            {
                ClientApplicationName = clientApplicationName
            };
        }
    }
}
