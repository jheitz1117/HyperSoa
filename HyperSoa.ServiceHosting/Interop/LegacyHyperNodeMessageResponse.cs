using HyperSoa.Contracts;

namespace HyperSoa.ServiceHosting.Interop
{
    internal class LegacyHyperNodeMessageResponse
    {
        public string? TaskId { get; set; }
        public string? RespondingNodeName { get; set; }
        public TimeSpan? TotalRunTime { get; set; }
        public HyperNodeActionType NodeAction { get; set; }
        public HyperNodeActionReasonType NodeActionReason { get; set; }
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
        public List<HyperNodeActivityItem> TaskTrace { get; set; }
        public string? CommandResponseString { get; set; }
    }
}
