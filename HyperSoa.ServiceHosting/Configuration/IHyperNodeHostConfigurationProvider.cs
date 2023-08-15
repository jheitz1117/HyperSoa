namespace HyperSoa.ServiceHosting.Configuration
{
    public interface IHyperNodeHostConfigurationProvider
    {
        public IEnumerable<IHyperNodeHttpEndpoint> GetHttpEndpoints();
        public IEnumerable<IHyperNodeTcpEndpoint> GetTcpEndpoints();
    }
}
