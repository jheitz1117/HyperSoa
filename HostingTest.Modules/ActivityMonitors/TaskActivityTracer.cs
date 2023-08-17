using System.Diagnostics;
using HyperSoa.Service.ActivityTracking;

namespace HostingTest.Modules.ActivityMonitors
{
    /// <summary>
    /// Stock <see cref="HyperNodeServiceActivityMonitor"/> that calls <see cref="Trace"/>.WriteLine() for each activity event. This monitor
    /// may be used during development along with an accompanying set of <see cref="TraceListener"/> objects to output activity to a file or
    /// console application. Alternatively, you can write your own subclass of <see cref="HyperNodeServiceActivityMonitor"/> to better suit
    /// your needs.
    /// </summary>
    public class TaskActivityTracer : HyperNodeServiceActivityMonitor
    {
        public TaskActivityTracer()
        {
            Name = nameof(TaskActivityTracer);
        }
        
        // TODO: How to switch to ILogger instead of Trace? Can I have the Activator insert an ilogger when I instantiate it?
        // TODO: Maybe see this: https://stackoverflow.com/questions/52644507/using-activatorutilities-createinstance-to-create-instance-from-type

        public override void OnTrack(IHyperNodeActivityEventItem activity)
        {
            Trace.WriteLine($"{activity.EventDateTime:G} {activity.EventDescription}");

            if (!string.IsNullOrWhiteSpace(activity.EventDetail))
                Trace.WriteLine($"    {activity.EventDetail}");
        }

        public override void OnActivityReportingError(Exception exception)
        {
            Trace.WriteLine(exception);
        }

        public override void OnTaskCompleted()
        {
            Trace.WriteLine("Task Completed!");
        }
    }
}
