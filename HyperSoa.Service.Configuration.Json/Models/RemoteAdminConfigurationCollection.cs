using System.Collections;

namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class RemoteAdminConfigurationCollection : IRemoteAdminConfigurationCollection
    {
        public bool Enabled { get; set; }

        public RemoteAdminCommandConfiguration[] Commands { get; set; } = Array.Empty<RemoteAdminCommandConfiguration>();

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
