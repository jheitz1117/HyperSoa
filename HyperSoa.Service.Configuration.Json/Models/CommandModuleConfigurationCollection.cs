using System.Collections;

namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class CommandModuleConfigurationCollection : ICommandModuleConfigurationCollection
    {
        public string? ContractSerializerType { get; set; }
        public List<CommandModuleConfiguration>? Commands { get; set; }

        public IEnumerator<ICommandModuleConfiguration> GetEnumerator()
        {
            return Commands?.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
