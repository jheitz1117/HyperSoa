using HyperSoa.Service.Configuration.Json.Models;
using Microsoft.Extensions.Configuration;

namespace HyperSoa.Service.Configuration.Json
{
    public class JsonHyperNodeConfigurationProvider : IHyperNodeConfigurationProvider
    {
        private readonly HyperNodeConfiguration _hyperNodeConfig;

        public JsonHyperNodeConfigurationProvider(IConfiguration config)
        {
            _hyperNodeConfig = config.GetRequiredSection("HyperSoa:ServiceConfig").Get<HyperNodeConfiguration>() ?? throw new InvalidOperationException($"Unable to deserialize {nameof(HyperNodeConfiguration)} from configuration.");
        }

        public IHyperNodeConfiguration GetConfiguration()
        {
            return _hyperNodeConfig;
        }
    }
}