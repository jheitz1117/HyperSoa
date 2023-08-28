using HyperSoa.Service.ActivityTracking.Trackers;
using Microsoft.Extensions.Logging;

namespace HyperSoa.Service.ActivityTracking
{
    /// <summary>
    /// Event arguments for <see cref="HyperNodeTaskActivityTracker"/> objects.
    /// </summary>
    internal class TrackActivityEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IActivityItem"/> describing the event.
        /// </summary>
        public IActivityItem ActivityItem { get; }

        /// <summary>
        /// The <see cref="Microsoft.Extensions.Logging.LogLevel"/> at which to log the event.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Initializes an instance of <see cref="TrackActivityEventArgs"/> using the specified <see cref="IActivityItem"/>.
        /// </summary>
        /// <param name="activityItem">The <see cref="IActivityItem"/> describing the event.</param>
        public TrackActivityEventArgs(IActivityItem activityItem)
            : this(activityItem, LogLevel.Trace) { }

        /// <summary>
        /// Initializes an instance of <see cref="TrackActivityEventArgs"/> using the specified <see cref="IActivityItem"/>.
        /// </summary>
        /// <param name="activityItem">The <see cref="IActivityItem"/> describing the event.</param>
        /// <param name="logLevel">The <see cref="Microsoft.Extensions.Logging.LogLevel"/> at which to log the activity.</param>
        public TrackActivityEventArgs(IActivityItem activityItem, LogLevel logLevel)
        {
            ActivityItem = activityItem;
            LogLevel = logLevel;
        }
    }
}
