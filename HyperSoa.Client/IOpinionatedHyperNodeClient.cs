using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface IOpinionatedHyperNodeClient : IHyperNodeService
    {
        public string? ClientApplicationName { get; set; }
        public bool ReturnTaskTrace { get; set; }
        public bool CacheTaskProgress { get; set; }
    }
}
