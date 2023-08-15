using HyperSoa.Contracts;
using HyperSoa.ServiceHosting.Configuration;
using Microsoft.Extensions.Logging;

namespace HyperSoa.ServiceHosting
{
    public class HyperNodeServiceHost : IHyperNodeServiceHost
    {
        private readonly IHyperNodeChannel[] _channels;

        public HyperNodeServiceHost(IHyperNodeHostConfigurationProvider configProvider, IHyperNodeService serviceInstance, ILoggerFactory loggerFactory)
        {
            var channels = new List<IHyperNodeChannel>();

            var httpBindings = configProvider.GetHttpEndpoints().ToArray();
            var tcpBindings = configProvider.GetTcpEndpoints().ToArray();

            if (httpBindings.Any())
            {
                channels.Add(
                    new HyperNodeHttpChannel(
                        httpBindings,
                        serviceInstance,
                        loggerFactory.CreateLogger<HyperNodeHttpChannel>()
                    )
                );
            }

            if (tcpBindings.Any())
                throw new NotSupportedException("TCP endpoints are not currently supported.");

            _channels = channels.ToArray();
        }

        public IEnumerable<IHyperNodeChannel> GetChannels()
        {
            return _channels;
        }
    }
}
