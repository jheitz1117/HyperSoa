﻿using HyperSoa.Contracts;
using HyperSoa.Service.EventTracking;
using Microsoft.Extensions.Logging;

namespace HyperSoa.Service.ActivityTracking.Trackers
{
    /// <summary>
    /// Delegate to handle events fired by <see cref="HyperNodeTaskActivityTracker"/>.
    /// </summary>
    /// <param name="sender">The object that fired the event.</param>
    /// <param name="e">The <see cref="TrackActivityEventArgs"/> object containing the event data.</param>
    internal delegate void TrackActivityEventHandler(object sender, TrackActivityEventArgs e);

    internal sealed class HyperNodeTaskActivityTracker : ITaskActivityTracker
    {
        private readonly object _lock = new();
        private readonly ITaskEventContext _taskContext;
        private readonly IHyperNodeEventHandler _eventHandler;
        private readonly Action _cancelTaskAction;
        private readonly ILogger<HyperNodeService> _serviceLogger;

        /// <summary>
        /// Allows consumers to respond to activity events.
        /// </summary>
        public event TrackActivityEventHandler TrackActivityHandler = (_, _) => { };

        public HyperNodeTaskActivityTracker(ITaskEventContext taskContext, IHyperNodeEventHandler eventHandler, Action cancelTaskAction, ILogger<HyperNodeService> serviceLogger)
        {
            _taskContext = taskContext;
            _eventHandler = eventHandler;
            _cancelTaskAction = () =>
            {
                Track("Task cancellation requested from user-defined code.");
                cancelTaskAction();
            };
            _serviceLogger = serviceLogger;
        }

        public void TrackTaskStarted()
        {
            Track("Task started.");

            try
            {
                _eventHandler.OnTaskStarted(
                    new TaskStartedEventArgs(this, _taskContext, _cancelTaskAction)
                );
            }
            catch (Exception ex)
            {
                TrackException(
                    ex,
                    $"An exception was thrown from the {nameof(IHyperNodeEventHandler.OnTaskStarted)} event. See the {nameof(IActivityItem.EventDetail)} property for details."
                );
            }
        }

        public void TrackMessageProcessed()
        {
            Track("Message processed.");

            try
            {
                _eventHandler.OnMessageProcessed(
                    new HyperNodeEventArgs(this, _taskContext)
                );
            }
            catch (Exception ex)
            {
                TrackException(
                    ex,
                    $"An exception was thrown from the {nameof(IHyperNodeEventHandler.OnMessageProcessed)} event. See the {nameof(IActivityItem.EventDetail)} property for details."
                );
            }
        }

        /// <summary>
        /// This method should only ever be called once at the very end of a HyperNode's processing of a message after all of the child threads have completed.
        /// </summary>
        /// <param name="response">The complete <see cref="HyperNodeMessageResponse"/> object to report.</param>
        public void TrackTaskComplete(HyperNodeMessageResponse response)
        {
            Track("Task complete.", null, response);

            try
            {
                _eventHandler.OnTaskCompleted(
                    new TaskCompletedEventArgs(
                        this,
                        _taskContext,
                        new ReadOnlyHyperNodeResponseInfo(response)
                    )
                );
            }
            catch (Exception ex)
            {
                TrackException(
                    ex,
                    $"An exception was thrown from the {nameof(IHyperNodeEventHandler.OnTaskCompleted)} event. See the {nameof(IActivityItem.EventDetail)} property for details."
                );
            }
        }

        public void TrackActivityVerbatim(HyperNodeActivityEventItem item)
        {
            OnTrackActivity(
                new TrackActivityEventArgs(item)
            );
        }

        public void Track(LogLevel logLevel, string? eventDescription)
        {
            Track(logLevel, eventDescription, null);
        }

        public void Track(LogLevel logLevel, string? eventDescription, string? eventDetail)
        {
            Track(logLevel, eventDescription, eventDetail, null);
        }

        public void Track(LogLevel logLevel, string? eventDescription, string? eventDetail, object? eventData)
        {
            Track(logLevel, eventDescription, eventDetail, eventData, null, null);
        }

        public void Track(LogLevel logLevel, string? eventDescription, double? progressPart, double? progressTotal)
        {
            Track(logLevel, eventDescription, null, progressPart, progressTotal);
        }

        public void Track(LogLevel logLevel, string? eventDescription, string? eventDetail, double? progressPart, double? progressTotal)
        {
            Track(logLevel, eventDescription, eventDetail, null, progressPart, progressTotal);
        }

        public void TrackFormat(LogLevel logLevel, string eventDescriptionFormat, params object?[] args)
        {
            Track(logLevel, string.Format(eventDescriptionFormat, args));
        }

        public void Track(LogLevel logLevel, string? eventDescription, string? eventDetail, object? eventData, double? progressPart, double? progressTotal)
        {
            OnTrackActivity(
                new TrackActivityEventArgs(
                    new HyperNodeActivityEventItem
                    {
                        Agent = _taskContext.ExecutingNodeName,
                        TaskId = _taskContext.TaskId,
                        CommandName = _taskContext.CommandName,
                        Elapsed = _taskContext.Elapsed,
                        EventDateTime = DateTime.Now,
                        EventDescription = eventDescription,
                        EventDetail = eventDetail,
                        EventData = eventData,
                        ProgressPart = progressPart,
                        ProgressTotal = progressTotal
                    },
                    logLevel
                )
            );
        }

        #region ITaskActivityTracker Implementation

        public void Track(string? eventDescription)
        {
            Track(LogLevel.Trace, eventDescription);
        }

        public void Track(string? eventDescription, string? eventDetail)
        {
            Track(LogLevel.Trace, eventDescription, eventDetail);
        }

        public void Track(string? eventDescription, string? eventDetail, object? eventData)
        {
            Track(LogLevel.Trace, eventDescription, eventDetail, eventData);
        }

        public void Track(string? eventDescription, double? progressPart, double? progressTotal)
        {
            Track(LogLevel.Trace, eventDescription, progressPart, progressTotal);
        }

        public void Track(string? eventDescription, string? eventDetail, double? progressPart, double? progressTotal)
        {
            Track(LogLevel.Trace, eventDescription, eventDetail, progressPart, progressTotal);
        }

        public void Track(string? eventDescription, string? eventDetail, object? eventData, double? progressPart, double? progressTotal)
        {
            Track(LogLevel.Trace, eventDescription, eventDetail, eventData, progressPart, progressTotal);
        }

        public void TrackFormat(string eventDescriptionFormat, params object?[] args)
        {
            TrackFormat(LogLevel.Trace, string.Format(eventDescriptionFormat, args));
        }

        public void TrackException(Exception exception)
        {
            Track(LogLevel.Error, exception.ToString());
        }

        public void TrackException(Exception exception, string? eventDescription)
        {
            Track(LogLevel.Error, eventDescription, exception.ToString());
        }

        #endregion ITaskActivityTracker Implementation

        #region Private Methods

        /// <summary>
        /// Fires an event containing the specified <see cref="TrackActivityEventArgs"/> object.
        /// </summary>
        /// <param name="e">The <see cref="TrackActivityEventArgs"/> data for the event.</param>
        private void OnTrackActivity(TrackActivityEventArgs e)
        {
            TrackActivityEventHandler handler;
            lock (_lock)
            {
                handler = TrackActivityHandler;
            }

            _serviceLogger.Log(e.LogLevel, "{m}", e.ActivityItem.EventDescription);

            if (!string.IsNullOrWhiteSpace(e.ActivityItem.EventDetail))
                _serviceLogger.Log(e.LogLevel, "{m}", e.ActivityItem.EventDetail);

            handler.Invoke(this, e);
        }

        #endregion Private Methods
    }
}
