namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class ActivityMonitorConfiguration : IActivityMonitorConfiguration
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool Enabled { get; set; }

        string? IActivityMonitorConfiguration.MonitorName => Name;
        string? IActivityMonitorConfiguration.MonitorType => Type;
    }
}
