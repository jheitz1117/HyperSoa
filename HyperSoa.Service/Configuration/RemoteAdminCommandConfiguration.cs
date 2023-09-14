namespace HyperSoa.Service.Configuration
{
    public class RemoteAdminCommandConfiguration : IRemoteAdminCommandConfiguration
    {
        public string? Name { get; set; }
        public bool Enabled { get; set; }

        string? IRemoteAdminCommandConfiguration.CommandName => Name;
    }
}
