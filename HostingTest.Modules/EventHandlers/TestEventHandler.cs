using HyperSoa.Service.EventTracking;

namespace HostingTest.Modules.EventHandlers
{
    public class TestEventHandler : HyperNodeEventHandlerBase
    {
        public override void OnMessageReceived(IMessageReceivedEventArgs args)
        {
            args.Activity.Track("OnMessageReceived Event");

            if (args.TaskContext.CommandName == "RejectedCommand")
            {
                args.RejectMessage("Rejected!");
            }
        }

        public override void OnTaskStarted(ITaskStartedEventArgs args)
        {
            args.Activity.Track("OnTaskStarted Event");

            if (args.TaskContext.CommandName == "LongRunningCommand")
            {
                //args.CancelTask();
            }
        }

        public override void OnMessageProcessed(IHyperNodeEventArgs args)
        {
            args.Activity.Track("OnMessageProcessed Event");
        }

        public override void OnTaskCompleted(ITaskCompletedEventArgs args)
        {
            args.Activity.Track("OnTaskCompleted Event");
        }
    }
}
