using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface IOpinionatedHyperNodeClient : IHyperNodeService
    {
        public string? ClientApplicationName { get; set; }
    }
}
