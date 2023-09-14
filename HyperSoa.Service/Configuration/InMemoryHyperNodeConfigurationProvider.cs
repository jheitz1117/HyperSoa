namespace HyperSoa.Service.Configuration
{
    public class InMemoryHyperNodeConfigurationProvider : IHyperNodeConfigurationProvider
    {
        private readonly IHyperNodeConfiguration _config;

        public InMemoryHyperNodeConfigurationProvider(IHyperNodeConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IHyperNodeConfiguration GetConfiguration()
        {
            return _config;
        }
    }
}
