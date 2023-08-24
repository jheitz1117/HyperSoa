namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class RemoteAdminCommandConfiguration : IRemoteAdminCommandConfiguration
    {
        public string? Name { get; set; }
        public bool Enabled { get; set; }

        string? IRemoteAdminCommandConfiguration.CommandName => Name;
    }
}
