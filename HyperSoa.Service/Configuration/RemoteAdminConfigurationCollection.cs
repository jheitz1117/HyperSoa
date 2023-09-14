using System.Collections;

namespace HyperSoa.Service.Configuration
{
    public class RemoteAdminConfigurationCollection : IRemoteAdminConfigurationCollection
    {
        public bool Enabled { get; set; }

        public List<RemoteAdminCommandConfiguration> Commands { get; set; } = new();

        public IEnumerator<IRemoteAdminCommandConfiguration> GetEnumerator()
        {
            return Commands.OfType<RemoteAdminCommandConfiguration>().GetEnumerator();
        }

        public IRemoteAdminCommandConfiguration? GetByCommandName(string commandName)
        {
            return Commands.FirstOrDefault(c => c.Name == commandName);
        }

        public bool ContainsCommandName(string commandName)
        {
            return Commands.Any(c => c.Name == commandName);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
