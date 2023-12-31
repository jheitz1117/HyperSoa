﻿using HyperSoa.Service.CommandModules;

namespace HyperSoa.Service.ActivityTracking
{
    /// <summary>
    /// Provides methods to raise <see cref="IHyperNodeActivityEventItem"/> events from inside a <see cref="ICommandModule"/>.
    /// </summary>
    public interface ITaskActivityTracker
    {
        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        void Track(string? eventDescription);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description and detail.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        /// <param name="eventDetail">A description providing more verbose details of the event.</param>
        void Track(string? eventDescription, string? eventDetail);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description and detail.
        /// This overload associates the specified object with the event data.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        /// <param name="eventDetail">A description providing more verbose details of the event.</param>
        /// <param name="eventData">An object to be associated with the <see cref="IHyperNodeActivityEventItem"/>.</param>
        void Track(string? eventDescription, string? eventDetail, object? eventData);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description and progress report.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        /// <param name="progressPart">A numeric value representing the progress of the <see cref="ICommandModule"/>.</param>
        /// <param name="progressTotal">A numeric value representing the progress total of a <see cref="ICommandModule"/>.</param>
        void Track(string? eventDescription, double? progressPart, double? progressTotal);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description, detail, and progress report.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        /// <param name="eventDetail">A description providing more verbose details of the event.</param>
        /// <param name="progressPart">A numeric value representing the progress of the <see cref="ICommandModule"/>.</param>
        /// <param name="progressTotal">A numeric value representing the progress total of a <see cref="ICommandModule"/>.</param>
        void Track(string? eventDescription, string? eventDetail, double? progressPart, double? progressTotal);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified description, detail, event data, and progress report.
        /// </summary>
        /// <param name="eventDescription">A description of the event.</param>
        /// <param name="eventDetail">A description providing more verbose details of the event.</param>
        /// <param name="eventData">An object to be associated with the <see cref="IHyperNodeActivityEventItem"/>.</param>
        /// <param name="progressPart">A numeric value representing the progress of the <see cref="ICommandModule"/>.</param>
        /// <param name="progressTotal">A numeric value representing the progress total of a <see cref="ICommandModule"/>.</param>
        void Track(string? eventDescription, string? eventDetail, object? eventData, double? progressPart, double? progressTotal);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> with the specified format string and parameters.
        /// </summary>
        /// <param name="eventDescriptionFormat">A format string description of the event.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void TrackFormat(string eventDescriptionFormat, params object?[] args);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> containing information about the specified <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to track.</param>
        void TrackException(Exception exception);

        /// <summary>
        /// Raises an <see cref="IHyperNodeActivityEventItem"/> containing information about the specified <see cref="Exception"/>.
        /// The exception information is included in the <see cref="IActivityItem.EventDetail"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to track.</param>
        /// <param name="eventDescription">A description of the error.</param>
        void TrackException(Exception exception, string? eventDescription);
    }
}
