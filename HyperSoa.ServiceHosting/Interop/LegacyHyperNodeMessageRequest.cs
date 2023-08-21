using HyperSoa.Contracts;

namespace HyperSoa.ServiceHosting.Interop
{
    internal class LegacyHyperNodeMessageRequest
    {
        public string? CreatedByAgentName { get; set; }
        public string? CommandName { get; set; }
        public string? CommandRequestString { get; set; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }
    }
}
