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

            var invalidHttpEndpoints = _httpEndpoints.Where(
                e =>
                {
                    var isAbsoluteUri = Uri.TryCreate(e.Uri, UriKind.Absolute, out var uri);
                    var isHttpUri = uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps;

                    return !(isAbsoluteUri && isHttpUri);
                }
            ).ToArray();

            if (invalidHttpEndpoints.Any())
                throw new InvalidOperationException($"The following HTTP endpoints were invalid: '{string.Join("', '", invalidHttpEndpoints.Select(ie => ie.Uri))}'");

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