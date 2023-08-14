namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class RemoteAdminCommandConfiguration : ISystemCommandConfiguration
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }

        string ISystemCommandConfiguration.CommandName => Name;
    }
}
