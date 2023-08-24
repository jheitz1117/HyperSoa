using HyperSoa.Contracts;
using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.EventTracking
{
    /// <summary>
    /// Event arguments passed to an implementation of <see cref="IHyperNodeEventHandler"/> when the <see cref="IHyperNodeService"/> fires an event.
    /// </summary>
    public interface IHyperNodeEventArgs
    {
        /// <summary>
        /// Provides a way to raise activity events from inside an <see cref="IHyperNodeEventHandler"/>.
        /// </summary>
        ITaskActivityTracker Activity { get; }

        /// <summary>
        /// Contains information about the current task being executed.
        /// </summary>
        ITaskEventContext TaskContext { get; }
    }
}
