using HyperSoa.Service.ActivityTracking;
using Microsoft.Extensions.Logging;

namespace HostingTest.Modules.ActivityMonitors
{
    /// <summary>
    /// Stock <see cref="HyperNodeServiceActivityMonitor"/> that calls <see cref="ILogger{T}"/>.LogTrace()
    /// for each activity event. This monitor may be used during development along with an accompanying
    /// set of log sinks to output activity to a file or console application. Alternatively, you can write
    /// your own subclass of <see cref="HyperNodeServiceActivityMonitor"/> to better suit your needs.
    /// </summary>
    public class TaskActivityLogger : HyperNodeServiceActivityMonitor
    {
        private readonly ILogger<TaskActivityLogger>? _logger;

        public TaskActivityLogger(ILogger<TaskActivityLogger> logger)
        {
            _logger = logger;
        }
        
        public override void OnTrack(IHyperNodeActivityEventItem activity)
        {
            _logger?.LogTrace("{edt:G} {ed}", activity.EventDateTime, activity.EventDescription);

            if (!string.IsNullOrWhiteSpace(activity.EventDetail))
                _logger?.LogTrace("     {ed}", activity.EventDetail);
        }

        public override void OnActivityReportingError(Exception exception)
        {
            _logger?.LogError(exception, "Error reporting activity.");
        }

        public override void OnTaskCompleted()
        {
            _logger?.LogTrace("Task Completed!");
        }
    }
}
