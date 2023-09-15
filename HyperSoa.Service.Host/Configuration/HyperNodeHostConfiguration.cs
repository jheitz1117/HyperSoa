using HyperSoa.Service.Configuration;

namespace HyperSoa.Service.Host.Configuration
{
    public class HyperNodeHostConfiguration
    {
        public const string ConfigurationSectionName = $"{HyperSoaConfiguration.ConfigurationSectionName}:{ScopedConfigurationSectionName}";
        public const string ScopedConfigurationSectionName = "HostConfig";

        public bool EnableLegacySoapSupport { get; set; }
        public HyperNodeHttpEndpoint[] HttpEndpoints { get; set; } = Array.Empty<HyperNodeHttpEndpoint>();
    }
}
