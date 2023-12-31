﻿using System.Configuration;

namespace HyperSoa.Service.Configuration.Xml.Models
{
    internal sealed class ActivityMonitorElementCollection : ConfigurationElementCollection, IActivityMonitorConfigurationCollection
    {
        public ActivityMonitorElement? this[int index]
        {
            get => BaseGet(index) as ActivityMonitorElement;
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new ActivityMonitorElement? this[string name]
        {
            get => BaseGet(name) as ActivityMonitorElement;
            set
            {
                if (BaseGet(name) != null)
                    BaseRemove(name);
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ActivityMonitorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActivityMonitorElement)element).MonitorName;
        }

        public new IEnumerator<IActivityMonitorConfiguration> GetEnumerator()
        {
            return this.OfType<ActivityMonitorElement>().GetEnumerator();
        }
    }
}
