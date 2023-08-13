using System.Collections;

namespace HyperSoa.Service.Configuration
{
    internal class EmptyActivityMonitorConfigurationCollection : IActivityMonitorConfigurationCollection
    {
        private readonly IEnumerable<IActivityMonitorConfiguration> _empty = Array.Empty<IActivityMonitorConfiguration>();

        public IEnumerator<IActivityMonitorConfiguration> GetEnumerator()
        {
            return _empty.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
