namespace HyperSoa.ServiceHosting.Configuration
{
    public interface IHyperNodeTcpEndpoint
    {
        public string Name { get; }
        public string IpAddress { get; }
        public int PortNumber { get; }
    }
}
