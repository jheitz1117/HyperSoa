﻿using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;

namespace HyperSoa.Service.ActivityTracking
{
    /// <summary>
    /// Describes an <see cref="IActivityItem"/> event raised by a <see cref="IHyperNodeService"/>.
    /// </summary>
    public interface IHyperNodeActivityEventItem : IActivityItem
    {
        /// <summary>
        /// The ID of the current task.
        /// </summary>
        string? TaskId { get; }

        /// <summary>
        /// The name of the command currently executing.
        /// </summary>
        string? CommandName { get; }

        /// <summary>
        /// The amount of time that has elapsed since the first <see cref="IHyperNodeActivityEventItem"/> was raised for this task.
        /// This value may be null unless diagnostics are enabled.
        /// </summary>
        TimeSpan? Elapsed { get; }

        /// <summary>
        /// An object representing data associated with this <see cref="IHyperNodeActivityEventItem"/>. This value may be null.
        /// </summary>
        object? EventData { get; }

        /// <summary>
        /// A numeric value representing the progress of a <see cref="ICommandModule"/>. This value may be used in conjunction
        /// with the <see cref="ProgressTotal"/> property to obtain a percentile.
        /// </summary>
        double? ProgressPart { get; }

        /// <summary>
        /// A numeric value representing the progress total of a <see cref="ICommandModule"/>. This value may be used in conjunction
        /// with the <see cref="ProgressPart"/> property to obtain a percentile.
        /// </summary>
        double? ProgressTotal { get; }
    }
}
