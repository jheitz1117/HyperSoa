namespace HyperSoa.ServiceHosting.Configuration
{
    public interface IHyperNodeHostConfigurationProvider
    {
        public IHyperNodeHostConfiguration? GetConfiguration();
    }
}
