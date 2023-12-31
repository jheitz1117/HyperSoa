﻿using HyperSoa.Contracts;

namespace HyperSoa.Service
{
    /// <summary>
    /// This class provides a read-only version of a <see cref="HyperNodeMessageRequest"/> for use in user-defined code.
    /// </summary>
    internal class ReadOnlyHyperNodeMessageInfo : IReadOnlyHyperNodeMessageInfo
    {
        public string? CommandName { get; }
        public string? CreatedByAgentName { get; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; }
        public byte[]? CommandRequestBytes { get; }

        public ReadOnlyHyperNodeMessageInfo(HyperNodeMessageRequest message)
        {
            CommandName = message.CommandName;
            CreatedByAgentName = message.CreatedByAgentName;
            ProcessOptionFlags = message.ProcessOptionFlags;
            CommandRequestBytes = message.CommandRequestBytes;
        }
    }
}
