namespace HyperSoa.Service.ActivityTracking.Trackers
{
    /// <summary>
    /// Delegate to handle events fired by <see cref="HyperActivityTracker"/> implementations.
    /// </summary>
    /// <param name="sender">The object that fired the event.</param>
    /// <param name="e">The <see cref="TrackActivityEventArgs"/> object containing the event data.</param>
    internal delegate void TrackActivityEventHandler(object sender, TrackActivityEventArgs e);

    /// <summary>
    /// Base class for tracking arbitrary activity events.
    /// </summary>
    internal abstract class HyperActivityTracker
    {
        private readonly object _lock = new();

        /// <summary>
        /// Allows consumers to respond to activity events.
        /// </summary>
        public event TrackActivityEventHandler TrackActivityHandler = (_, _) => { };

        /// <summary>
        /// When overridden in a derived class, fires an event containing the specified <see cref="TrackActivityEventArgs"/> object.
        /// </summary>
        /// <param name="e">The <see cref="TrackActivityEventArgs"/> data for the event.</param>
        protected virtual void OnTrackActivity(TrackActivityEventArgs e)
        {
            TrackActivityEventHandler handler;
            lock (_lock)
            {
                handler = TrackActivityHandler;
            }

            handler.Invoke(this, e);
        }
    }
}
