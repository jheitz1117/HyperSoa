using System.Collections;
using HyperSoa.Contracts.RemoteAdmin;

namespace HyperSoa.Service.Configuration
{
    internal class FullAdminSystemCommandModuleConfigurationCollection : ISystemCommandConfigurationCollection
    {
        private readonly IDictionary<string, SystemCommandConfiguration> _systemCommands;

        public bool Enabled => true;

        public FullAdminSystemCommandModuleConfigurationCollection()
        {
            _systemCommands = RemoteAdminCommandName.GetAll().ToDictionary(
                commandName => commandName,
                commandName => new SystemCommandConfiguration(commandName, true)
            );
        }

        public IEnumerator<ISystemCommandConfiguration> GetEnumerator()
        {
            return _systemCommands.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ISystemCommandConfiguration GetByCommandName(string commandName)
        {
            _systemCommands.TryGetValue(commandName, out var result);
            
            return result;
        }

        public bool ContainsCommandName(string commandName)
        {
            return _systemCommands.ContainsKey(commandName);
        }
    }
}
