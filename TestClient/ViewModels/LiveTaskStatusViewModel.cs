namespace TestClient.ViewModels
{
    public class LiveTaskStatusViewModel
    {
        public string? TaskId { get; set; }
        public string? CommandName { get; set; }
        public string? Status { get; set; }
        public TimeSpan? Elapsed { get; set; }
    }
}
