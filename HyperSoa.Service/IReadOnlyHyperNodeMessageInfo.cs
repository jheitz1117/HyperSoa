using HyperSoa.Contracts;

namespace HyperSoa.Service
{
    /// <summary>
    /// Contains information from <see cref="HyperNodeMessageRequest"/> objects being processed by <see cref="IHyperNodeService"/> instances.
    /// </summary>
    public interface IReadOnlyHyperNodeMessageInfo
    {
        /// <summary>
        /// The name of the command specified in the <see cref="HyperNodeMessageRequest"/>.
        /// </summary>
        string? CommandName { get; }

        /// <summary>
        /// The name of the agent that created the <see cref="HyperNodeMessageRequest"/>.
        /// </summary>
        string? CreatedByAgentName { get; }

        /// <summary>
        /// The processing flags specified in the <see cref="IHyperNodeService"/>.
        /// </summary>
        MessageProcessOptionFlags ProcessOptionFlags { get; }

        /// <summary>
        /// Contains the request bytes specified in the <see cref="HyperNodeMessageRequest"/>.
        /// </summary>
        public byte[]? CommandRequestBytes { get; }
    }
}
