namespace HyperSoa.Service.ActivityTracking.Trackers
{
    internal sealed class NullActivityTracker : ITaskActivityTracker
    {
        private static readonly object Lock = new();

        private static NullActivityTracker? _instance;

        public static NullActivityTracker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        _instance ??= new NullActivityTracker();
                    }
                }

                return _instance;
            }
        }

        private NullActivityTracker() { }

        public void Track(string? eventDescription) { }

        public void Track(string? eventDescription, string? eventDetail) { }

        public void Track(string? eventDescription, string? eventDetail, object? eventData) { }

        public void Track(string? eventDescription, double? progressPart, double? progressTotal) { }

        public void Track(string? eventDescription, string? eventDetail, double? progressPart, double? progressTotal) { }

        public void Track(string? eventDescription, string? eventDetail, object? eventData, double? progressPart, double? progressTotal) { }

        public void TrackFormat(string eventDescriptionFormat, params object?[] args) { }

        public void TrackException(Exception exception) { }
    }
}
