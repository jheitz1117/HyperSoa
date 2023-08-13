﻿using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class ActivityMonitorElement : ConfigurationElement, IActivityMonitorConfiguration
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string MonitorName
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string MonitorType
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
    }
}
