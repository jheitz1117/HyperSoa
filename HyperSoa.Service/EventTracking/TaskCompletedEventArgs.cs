using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.EventTracking
{
    internal sealed class TaskCompletedEventArgs : HyperNodeEventArgs, ITaskCompletedEventArgs
    {
        public IReadOnlyHyperNodeResponseInfo ResponseInfo { get; }

        public TaskCompletedEventArgs(ITaskActivityTracker activity, ITaskEventContext taskContext, IReadOnlyHyperNodeResponseInfo responseInfo)
            : base(activity, taskContext)
        {
            ResponseInfo = responseInfo;
        }
    }
}
