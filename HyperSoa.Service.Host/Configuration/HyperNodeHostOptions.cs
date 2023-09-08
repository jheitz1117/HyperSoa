namespace HyperSoa.Service.Host.Configuration
{
    public class HyperNodeHostOptions
    {
        public const string ConfigurationSectionName = "HyperSoa:HostConfig";

        public bool UseInteropHttpChannel { get; set; }
        public HyperNodeHttpEndpoint[] HttpEndpoints { get; set; } = Array.Empty<HyperNodeHttpEndpoint>();
    }
}
