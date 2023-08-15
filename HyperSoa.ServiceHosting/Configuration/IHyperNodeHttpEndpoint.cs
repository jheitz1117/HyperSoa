namespace HyperSoa.ServiceHosting.Configuration
{
    public interface IHyperNodeHttpEndpoint
    {
        public string? Name { get; }
        public string? Uri { get; }
    }
}
