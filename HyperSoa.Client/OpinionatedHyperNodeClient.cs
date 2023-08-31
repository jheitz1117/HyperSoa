using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    internal class OpinionatedHyperNodeClient : OpinionatedHyperNodeClientBase
    {
        public OpinionatedHyperNodeClient(IHyperNodeService underlyingService)
            : base(underlyingService) { }
    }
}
