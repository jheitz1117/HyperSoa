using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.EventTracking
{
    internal sealed class TaskStartedEventArgs : HyperNodeEventArgs, ITaskStartedEventArgs
    {
        private readonly Action? _cancelTaskAction;

        public TaskStartedEventArgs(ITaskActivityTracker activity, ITaskEventContext taskContext, Action? cancelTaskAction)
            :base(activity, taskContext)
        {
            _cancelTaskAction = cancelTaskAction;
        }

        public void CancelTask()
        {
            _cancelTaskAction?.Invoke();
        }
    }
}
