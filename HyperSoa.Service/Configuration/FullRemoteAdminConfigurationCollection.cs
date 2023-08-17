using System.Collections;
using HyperSoa.Contracts.RemoteAdmin;

namespace HyperSoa.Service.Configuration
{
    internal class FullRemoteAdminConfigurationCollection : IRemoteAdminConfigurationCollection
    {
        private readonly IDictionary<string, RemoteAdminConfiguration> _remoteAdminCommands;

        public bool Enabled => true;

        public FullRemoteAdminConfigurationCollection()
        {
            _remoteAdminCommands = RemoteAdminCommandName.GetAll().ToDictionary(
                commandName => commandName,
                commandName => new RemoteAdminConfiguration(commandName, true)
            );
        }

        public IEnumerator<IRemoteAdminCommandConfiguration> GetEnumerator()
        {
            return _remoteAdminCommands.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IRemoteAdminCommandConfiguration GetByCommandName(string commandName)
        {
            return _remoteAdminCommands[commandName];
        }

        public bool ContainsCommandName(string commandName)
        {
            return _remoteAdminCommands.ContainsKey(commandName);
        }
    }
}
