using HyperSoa.ServiceHosting.Configuration.Json.Models;
using Microsoft.Extensions.Configuration;

namespace HyperSoa.ServiceHosting.Configuration.Json
{
    public class JsonHyperNodeHostConfigurationProvider : IHyperNodeHostConfigurationProvider
    {
        private readonly IHyperNodeHostConfiguration? _hostConfig;

        public JsonHyperNodeHostConfigurationProvider(IConfiguration config)
        {
            var hostConfig = config.GetSection("HyperNodeHost").Get<JsonHyperNodeHostConfiguration>();

            hostConfig?.Validate(true);

            _hostConfig = hostConfig;
        }

        public IHyperNodeHostConfiguration? GetConfiguration()
        {
            return _hostConfig;
        }
    }
}