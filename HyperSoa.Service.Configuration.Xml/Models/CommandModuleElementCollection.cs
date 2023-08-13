using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class CommandModuleElementCollection : ConfigurationElementCollection, ICommandModuleConfigurationCollection
    {
        public CommandModuleElement this[int index]
        {
            get => BaseGet(index) as CommandModuleElement;
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new CommandModuleElement this[string name]
        {
            get => BaseGet(name) as CommandModuleElement;
            set
            {
                if (BaseGet(name) != null)
                    BaseRemove(name);
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CommandModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommandModuleElement)element).CommandName;
        }

        public new IEnumerator<ICommandModuleConfiguration> GetEnumerator()
        {
            return this.OfType<CommandModuleElement>().GetEnumerator();
        }

        [ConfigurationProperty("contractSerializer", IsRequired = false)]
        public string ContractSerializerType
        {
            get => (string)base["contractSerializer"];
            set => base["contractSerializer"] = value;
        }
    }
}
