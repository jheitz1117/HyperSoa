namespace HyperSoa.Service.Configuration
{
    internal class InMemoryHyperNodeConfigurationProvider : IHyperNodeConfigurationProvider
    {
        public IHyperNodeConfiguration GetConfiguration()
        {
            return new InMemoryHyperNodeConfiguration();
        }
    }
}
