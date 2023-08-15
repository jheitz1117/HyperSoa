using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class SystemCommandElement : ConfigurationElement, IRemoteAdminCommandConfiguration
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string CommandName
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("enabled", IsRequired = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }
    }
}
