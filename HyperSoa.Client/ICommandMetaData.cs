namespace HyperSoa.Client
{
    public interface ICommandMetaData
    {
        public string? CreatedByAgentName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
        public HyperNodeResponseHandler? ResponseHandler { get; set; }
        public byte[]? GetCommandRequestBytes();
    }
}
