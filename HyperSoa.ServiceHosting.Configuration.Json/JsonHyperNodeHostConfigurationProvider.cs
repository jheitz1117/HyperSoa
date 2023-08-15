using HyperSoa.ServiceHosting.Configuration.Json.Models;
using Microsoft.Extensions.Configuration;

namespace HyperSoa.ServiceHosting.Configuration.Json
{
    public class JsonHyperNodeHostConfigurationProvider : IHyperNodeHostConfigurationProvider
    {
        private readonly HyperNodeHttpEndpoint[] _httpEndpoints;
        private readonly IHyperNodeTcpEndpoint[] _tcpEndpoints;

        public JsonHyperNodeHostConfigurationProvider(IConfiguration config)
        {
            _httpEndpoints = config
                .GetSection("HyperNodeHost")
                .GetSection("HttpEndpoints")
                .Get<HyperNodeHttpEndpoint[]>() ?? Array.Empty<HyperNodeHttpEndpoint>();

            _tcpEndpoints = Array.Empty<IHyperNodeTcpEndpoint>();
            if (config.GetSection("HyperNodeHost").GetSection("TcpEndpoints").Exists())
                throw new NotSupportedException("TCP endpoints are not currently supported.");
        }

        public IEnumerable<IHyperNodeHttpEndpoint> GetHttpEndpoints()
        {
            return _httpEndpoints;
        }

        public IEnumerable<IHyperNodeTcpEndpoint> GetTcpEndpoints()
        {
            return _tcpEndpoints;
        }
    }
}