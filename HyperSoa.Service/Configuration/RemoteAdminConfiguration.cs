namespace HyperSoa.Service.Configuration
{
    internal class RemoteAdminConfiguration : IRemoteAdminCommandConfiguration
    {
        public string CommandName { get; set; }
        public bool Enabled { get; set; }

        public RemoteAdminConfiguration(string commandName, bool enabled)
        {
            CommandName = commandName;
            Enabled = enabled;
        }
    }
}
