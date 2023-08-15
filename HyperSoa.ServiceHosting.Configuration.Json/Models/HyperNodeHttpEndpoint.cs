namespace HyperSoa.ServiceHosting.Configuration.Json.Models
{
    internal class HyperNodeHttpEndpoint : IHyperNodeHttpEndpoint
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }
    }
}
