﻿using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Flags to indicate which processing options to use when the <see cref="IHyperNodeService"/> processes the <see cref="HyperNodeMessageRequest"/>.
    /// </summary>
    [Flags]
    [ProtoContract]
    public enum MessageProcessOptionFlags
    {
        /// <summary>
        /// Specifies that the recipient node should process this message synchronously without returning a task trace or caching activity events
        /// </summary>
        [ProtoEnum]
        None = 0,

        /// <summary>
        /// Specifies that the recipient node should return an activity event trace for this message.
        /// </summary>
        [ProtoEnum]
        ReturnTaskTrace = 1 << 0,

        /// <summary>
        /// Specifies that the recipient node should run the request in a child thread.
        /// </summary>
        [ProtoEnum]
        RunConcurrently = 1 << 1,

        /// <summary>
        /// Specifies that the recipient node should cache task progress for this message.
        /// </summary>
        [ProtoEnum]
        CacheTaskProgress = 1 << 2,
    }
}
