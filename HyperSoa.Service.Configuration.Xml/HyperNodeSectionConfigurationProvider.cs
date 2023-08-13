using System.Configuration;
using HyperSoa.Service.Configuration.Xml.Models;

namespace HyperSoa.Service.Configuration.Xml
{
    public class HyperNodeSectionConfigurationProvider : IHyperNodeConfigurationProvider
    {
        private const string HyperNodeConfigurationSectionName = "hyperSoa/hyperNode";

        public IHyperNodeConfiguration GetConfiguration()
        {
            var section = (HyperNodeConfigurationSection)ConfigurationManager.GetSection(HyperNodeConfigurationSectionName);
            if (section == null)
                throw new HyperNodeConfigurationException($"The application configuration file does not contain a section with the name '{HyperNodeConfigurationSectionName}'.");

            return section;
        }
    }
}