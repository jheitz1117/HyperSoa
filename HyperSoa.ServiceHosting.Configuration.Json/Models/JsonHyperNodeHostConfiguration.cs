namespace HyperSoa.ServiceHosting.Configuration.Json.Models
{
    internal class JsonHyperNodeHostConfiguration : IHyperNodeHostConfiguration
    {
        private bool _validated;

        public bool UseInteropHttpChannel { get; set; }

        public HyperNodeHttpEndpoint[]? HttpEndpoints { get; set; }
        public IHyperNodeTcpEndpoint[]? TcpEndpoints { get; set; }

        public IEnumerable<IHyperNodeHttpEndpoint> GetHttpEndpoints()
        {
            if (!_validated)
                Validate(true);

            return HttpEndpoints ?? Array.Empty<IHyperNodeHttpEndpoint>();
        }

        public IEnumerable<IHyperNodeTcpEndpoint> GetTcpEndpoints()
        {
            if (!_validated)
                Validate(true);

            return TcpEndpoints ?? Array.Empty<IHyperNodeTcpEndpoint>();
        }

        public void Validate(bool throwOnError)
        {
            var invalidHttpEndpoints = HttpEndpoints?.Where(
                e =>
                {
                    var isAbsoluteUri = Uri.TryCreate(e.Uri, UriKind.Absolute, out var uri);
                    var isHttpUri = uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps;

                    return !(isAbsoluteUri && isHttpUri);
                }
            ).ToArray() ?? Array.Empty<IHyperNodeHttpEndpoint>();

            if (invalidHttpEndpoints.Any() && throwOnError)
                throw new InvalidOperationException($"The following HTTP endpoints were invalid: '{string.Join("', '", invalidHttpEndpoints.Select(ie => ie.Uri))}'");

            if ((TcpEndpoints?.Any() ?? false) && throwOnError)
                throw new NotSupportedException("TCP endpoints are not currently supported.");

            _validated = true;
        }
    }
}
