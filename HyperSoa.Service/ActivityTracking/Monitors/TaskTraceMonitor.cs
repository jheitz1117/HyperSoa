using HyperSoa.Contracts;

namespace HyperSoa.Service.ActivityTracking.Monitors
{
    internal sealed class TaskTraceMonitor : HyperNodeServiceActivityMonitor
    {
        private static readonly object Lock = new();
        private readonly List<HyperNodeActivityItem> _taskTrace = new();

        public TaskTraceMonitor()
        {
            Name = nameof(TaskTraceMonitor);
        }

        public override void OnTrack(IHyperNodeActivityEventItem activity)
        {
            lock (Lock)
            {
                _taskTrace.Add(
                    new HyperNodeActivityItem
                    {
                        Agent = activity.Agent,
                        EventDateTime = activity.EventDateTime,
                        EventDescription = activity.EventDescription,
                        EventDetail = activity.EventDetail,
                        ProgressPart = activity.ProgressPart,
                        ProgressTotal = activity.ProgressTotal,
                        Elapsed = activity.Elapsed
                    }
                );
            }
        }

        public override void OnActivityReportingError(Exception exception)
        {
            lock (Lock)
            {
                _taskTrace.Add(
                    new HyperNodeActivityItem
                    {
                        Agent = Name,
                        EventDateTime = DateTime.Now,
                        EventDescription = exception.Message,
                        EventDetail = exception.ToString()
                    }
                );
            }
        }

        public HyperNodeActivityItem[] GetTaskTrace()
        {
            lock (Lock)
            {
                return _taskTrace.ToArray();
            }
        }
    }
}
