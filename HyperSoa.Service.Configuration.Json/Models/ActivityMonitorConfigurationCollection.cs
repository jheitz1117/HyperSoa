namespace HyperSoa.Service.Configuration.Json.Models
{
    internal class ActivityMonitorConfigurationCollection : List<ActivityMonitorConfiguration>, IActivityMonitorConfigurationCollection
    {
        public new IEnumerator<IActivityMonitorConfiguration> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
