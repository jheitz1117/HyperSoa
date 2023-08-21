namespace HyperSoa.ServiceHosting.Configuration
{
    public interface IHyperNodeHostConfiguration
    {
        public bool UseInteropHttpChannel { get; }
        public IEnumerable<IHyperNodeHttpEndpoint> GetHttpEndpoints();
        public IEnumerable<IHyperNodeTcpEndpoint> GetTcpEndpoints();
    }
}
