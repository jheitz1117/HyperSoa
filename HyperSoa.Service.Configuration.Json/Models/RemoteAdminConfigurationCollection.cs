using System.Collections;

namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class RemoteAdminConfigurationCollection : ISystemCommandConfigurationCollection
    {
        public bool Enabled { get; set; }
        public RemoteAdminCommandConfiguration[]? Commands { get; set; }

        public IEnumerator<ISystemCommandConfiguration> GetEnumerator()
        {
            return Commands?.OfType<RemoteAdminCommandConfiguration>().GetEnumerator();
        }

        public ISystemCommandConfiguration? GetByCommandName(string commandName)
        {
            return Commands?.FirstOrDefault(c => c.Name == commandName);
        }

        public bool ContainsCommandName(string commandName)
        {
            return Commands?.Any(c => c.Name == commandName) ?? false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
