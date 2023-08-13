using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class CommandModuleElement : ConfigurationElement, ICommandModuleConfiguration
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string CommandName
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string CommandModuleType
        {
            get => (string)this["type"];
            set => this["type"] = value;
        }

        [ConfigurationProperty("enabled", IsRequired = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("contractSerializer", IsRequired = false)]
        public string ContractSerializerType
        {
            get => (string)this["contractSerializer"];
            set => this["contractSerializer"] = value;
        }
    }
}
