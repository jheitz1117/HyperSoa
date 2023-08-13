using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.EventTracking
{
    internal class HyperNodeEventArgs : EventArgs, IHyperNodeEventArgs
    {
        public ITaskActivityTracker Activity { get; }
        public ITaskEventContext TaskContext { get; }

        public HyperNodeEventArgs(ITaskActivityTracker activity, ITaskEventContext taskContext)
        {
            Activity = activity;
            TaskContext = taskContext;
        }
    }
}
