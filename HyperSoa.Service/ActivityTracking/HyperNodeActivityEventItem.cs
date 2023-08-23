namespace HyperSoa.Service.ActivityTracking
{
    internal sealed class HyperNodeActivityEventItem : IHyperNodeActivityEventItem
    {
        public string? TaskId { get; set; }
        public string? CommandName { get; set; }
        public TimeSpan? Elapsed { get; set; }

        public DateTime EventDateTime { get; set; }
        public string? Agent { get; set; }
        public string? EventDescription { get; set; }
        public string? EventDetail { get; set; }
        public object? EventData { get; set; }
        public double? ProgressPart { get; set; }
        public double? ProgressTotal { get; set; }
    }
}
