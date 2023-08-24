using HyperSoa.Service.ActivityTracking.Trackers;

namespace HyperSoa.Service.ActivityTracking
{
    /// <summary>
    /// Event arguments for <see cref="HyperNodeTaskActivityTracker"/> objects.
    /// </summary>
    public class TrackActivityEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IActivityItem"/> describing the event.
        /// </summary>
        public IActivityItem ActivityItem { get; }

        /// <summary>
        /// Initializes an instance of <see cref="TrackActivityEventArgs"/> using the specified <see cref="IActivityItem"/>.
        /// </summary>
        /// <param name="activityItem">The <see cref="IActivityItem"/> describing the event.</param>
        public TrackActivityEventArgs(IActivityItem activityItem)
        {
            ActivityItem = activityItem;
        }
    }
}
