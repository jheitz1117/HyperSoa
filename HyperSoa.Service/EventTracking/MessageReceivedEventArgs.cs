using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.EventTracking
{
    internal sealed class MessageReceivedEventArgs : HyperNodeEventArgs, IMessageReceivedEventArgs
    {
        private readonly Action<string>? _rejectMessageAction;

        public MessageReceivedEventArgs(ITaskActivityTracker activity, ITaskEventContext taskContext, Action<string>? rejectMessageAction)
            : base(activity, taskContext)
        {
            _rejectMessageAction = rejectMessageAction;
        }

        public void RejectMessage(string reason)
        {
            _rejectMessageAction?.Invoke(reason);
        }
    }
}
