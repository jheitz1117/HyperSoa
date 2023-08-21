using HyperSoa.Contracts;
using HyperSoa.ServiceHosting.Configuration;
using HyperSoa.ServiceHosting.Interop;
using Microsoft.Extensions.Logging;

namespace HyperSoa.ServiceHosting
{
    public class HyperNodeServiceHost : IHyperNodeServiceHost
    {
        private readonly IHyperNodeChannel[] _channels;

        public HyperNodeServiceHost(IHyperNodeHostConfigurationProvider configProvider, IHyperNodeService serviceInstance, ILoggerFactory loggerFactory)
        {
            var channels = new List<IHyperNodeChannel>();

            var hostConfig = configProvider.GetConfiguration();

            var httpBindings = hostConfig?.GetHttpEndpoints().ToArray() ?? Array.Empty<IHyperNodeHttpEndpoint>();
            var tcpBindings = hostConfig?.GetTcpEndpoints().ToArray() ?? Array.Empty<IHyperNodeTcpEndpoint>();

            if (httpBindings.Any())
            {
                IHyperNodeChannel httpChannel;

                if (hostConfig?.UseInteropHttpChannel ?? false)
                {
                    httpChannel = new HyperNodeInteropHttpChannel(
                        httpBindings,
                        serviceInstance,
                        loggerFactory.CreateLogger<HyperNodeInteropHttpChannel>()
                    );
                }
                else
                {
                    httpChannel = new HyperNodeHttpChannel(
                        httpBindings,
                        serviceInstance,
                        loggerFactory.CreateLogger<HyperNodeHttpChannel>()
                    );
                }
                
                channels.Add(httpChannel);
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
