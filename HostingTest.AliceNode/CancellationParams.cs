namespace HostingTest.AliceNode
{
    internal class CancellationParams
    {
        public int MillisecondsTimeout { get; set; }
        public TimeSpan Timeout { get; set; }
        public CancellationToken Token { get; set; }
    }
}
