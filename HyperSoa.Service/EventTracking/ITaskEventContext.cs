﻿using HyperSoa.Contracts;

namespace HyperSoa.Service.EventTracking
{
    /// <summary>
    /// Exposes information about the current task running in the <see cref="IHyperNodeService"/>.
    /// </summary>
    public interface ITaskEventContext
    {
        /// <summary>
        /// The name of the <see cref="IHyperNodeService"/>.
        /// </summary>
        string ExecutingNodeName { get; }

        /// <summary>
        /// The name of the command being executed.
        /// </summary>
        string? CommandName { get; }

        /// <summary>
        /// The message being processed by the current task.
        /// </summary>
        IReadOnlyHyperNodeMessageInfo MessageInfo { get; }

        /// <summary>
        /// The Task ID for the current task.
        /// </summary>
        string? TaskId { get; }

        /// <summary>
        /// The amount of time which has elapsed since the task was started. If the <see cref="IHyperNodeService"/> has diagnostics turned off,
        /// then this property is null.
        /// </summary>
        TimeSpan? Elapsed { get; }
    }
}
