using HyperSoa.Contracts;

namespace HyperSoa.Service.ActivityTracking.Monitors
{
    internal class CachedTaskProgressInfo
    {
        public List<HyperNodeActivityItem> Activity { get; set; } = new();
        public HyperNodeMessageResponse? Response { get; set; }
        public bool IsComplete { get; set; }
        public double? ProgressPart { get; set; }
        public double? ProgressTotal { get; set; }
    }
}
