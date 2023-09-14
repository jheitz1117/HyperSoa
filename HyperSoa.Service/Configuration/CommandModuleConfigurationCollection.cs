using System.Collections;

namespace HyperSoa.Service.Configuration
{
    public class CommandModuleConfigurationCollection : ICommandModuleConfigurationCollection
    {
        public string? ContractSerializerType { get; set; }
        public List<CommandModuleConfiguration> Commands { get; set; } = new();

        public IEnumerator<ICommandModuleConfiguration> GetEnumerator()
        {
            return Commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
