using HyperSoa.Contracts;
using HyperSoa.Service.Host.Configuration;
using HyperSoa.Service.Host.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperSoa.Service.Host
{
    public class HyperNodeServiceHost : IHyperNodeServiceHost
    {
        private readonly IHyperNodeChannel[] _channels;

        public HyperNodeServiceHost(IOptions<HyperNodeHostConfiguration> options, IHyperNodeService serviceInstance, ILoggerFactory loggerFactory)
        {
            var channels = new List<IHyperNodeChannel>();

            var hostConfig = options.Value;

            if (hostConfig.HttpEndpoints.Any())
            {
                IHyperNodeChannel httpChannel;

                if (hostConfig.EnableLegacySoapSupport)
                {
                    httpChannel = new HyperNodeInteropHttpChannel(
                        hostConfig.HttpEndpoints,
                        serviceInstance,
                        loggerFactory.CreateLogger<HyperNodeInteropHttpChannel>()
                    );
                }
                else
                {
                    httpChannel = new HyperNodeHttpChannel(
                        hostConfig.HttpEndpoints,
                        serviceInstance,
                        loggerFactory.CreateLogger<HyperNodeHttpChannel>()
                    );
                }
                
                channels.Add(httpChannel);
            }

            _channels = channels.ToArray();
        }

        public IEnumerable<IHyperNodeChannel> GetChannels()
        {
            return _channels;
        }
    }
}
