namespace HyperSoa.Service.Configuration
{
    internal class SystemCommandConfiguration : ISystemCommandConfiguration
    {
        public string CommandName { get; set; }
        public bool Enabled { get; set; }

        public SystemCommandConfiguration(string commandName, bool enabled)
        {
            CommandName = commandName;
            Enabled = enabled;
        }
    }
}
