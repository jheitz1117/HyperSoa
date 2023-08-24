using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class SystemCommandElementCollection : ConfigurationElementCollection, IRemoteAdminConfigurationCollection
    {
        public SystemCommandElement? this[int index]
        {
            get => BaseGet(index) as SystemCommandElement;
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new SystemCommandElement? this[string name]
        {
            get => BaseGet(name) as SystemCommandElement;
            set
            {
                if (BaseGet(name) != null)
                    BaseRemove(name);
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SystemCommandElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SystemCommandElement)element).CommandName;
        }

        public new IEnumerator<IRemoteAdminCommandConfiguration> GetEnumerator()
        {
            return this.OfType<SystemCommandElement>().GetEnumerator();
        }

        [ConfigurationProperty("enabled", IsRequired = false)]
        public bool Enabled
        {
            get => (bool)base["enabled"];
            set => base["enabled"] = value;
        }

        public IRemoteAdminCommandConfiguration? GetByCommandName(string commandName)
        {
            return this[commandName];
        }

        public bool ContainsCommandName(string commandName)
        {
            return BaseGetAllKeys().Any(k => k.ToString() == commandName);
        }
    }
}
