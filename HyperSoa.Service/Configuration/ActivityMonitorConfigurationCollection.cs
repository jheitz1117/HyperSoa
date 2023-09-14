namespace HyperSoa.Service.Configuration
{
    public class ActivityMonitorConfigurationCollection : List<ActivityMonitorConfiguration>, IActivityMonitorConfigurationCollection
    {
        public new IEnumerator<IActivityMonitorConfiguration> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
