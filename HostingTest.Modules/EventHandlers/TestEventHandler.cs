using HyperSoa.Service.EventTracking;

namespace HostingTest.Modules.EventHandlers
{
    public class TestEventHandler : HyperNodeEventHandlerBase
    {
        public override void OnMessageReceived(IMessageReceivedEventArgs args)
        {
            if (args.TaskContext.CommandName == "RejectedCommand")
            {
                args.RejectMessage("Rejected!");
            }
        }

        public override void OnTaskStarted(ITaskStartedEventArgs args)
        {
            if (args.TaskContext.CommandName == "LongRunningCommand")
            {
                //args.CancelTask();
            }
        }

        public override void OnMessageProcessed(IHyperNodeEventArgs args)
        {
            args.Activity.Track($"Message processed by {args.TaskContext.ExecutingNodeName}");
        }

        public override void OnTaskCompleted(ITaskCompletedEventArgs args)
        {
            args.Activity.Track("From task completed event");
        }
    }
}
