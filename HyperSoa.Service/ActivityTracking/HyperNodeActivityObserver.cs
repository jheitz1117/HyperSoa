using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace HyperSoa.Service.ActivityTracking
{
    /// <summary>
    /// When disposed, uses the specified <see cref="IScheduler"/> to schedule the disposal of itself and the specified <see cref="HyperNodeServiceActivityMonitor"/>.
    /// </summary>
    internal sealed class HyperNodeActivityObserver : IObserver<IHyperNodeActivityEventItem>, IDisposable
    {
        private readonly HyperNodeServiceActivityMonitor _underlyingMonitor;
        private readonly ITaskActivityTracker? _activity;
        private readonly IDisposable? _subscription;
        private readonly IScheduler _scheduler;
        private readonly ILogger _logger;
        private bool _isDisposed;

        public HyperNodeActivityObserver(HyperNodeServiceActivityMonitor underlyingMonitor, IObservable<IHyperNodeActivityEventItem> activityEventStream, IScheduler scheduler, HyperNodeTaskInfo taskInfo, ILogger logger)
        {
            _underlyingMonitor = underlyingMonitor;
            _activity = taskInfo.Activity;
            _scheduler = scheduler;
            _logger = logger;
            _subscription = activityEventStream
                .Where(
                    e =>
                    {
                        var shouldTrack = false;

                        try
                        {
                            // ...for activity items matching the specified criteria
                            shouldTrack = underlyingMonitor.ShouldTrack(e);
                        }
                        catch (Exception ex)
                        {
                            // This is legal at this point because we already subscribed our cache and task trace monitors earlier
                            _activity?.TrackException(
                                ex,
                                $"Unable to subscribe activity monitor '{underlyingMonitor.Name}' because its {nameof(HyperNodeServiceActivityMonitor.ShouldTrack)}() method threw an exception. See the {nameof(IActivityItem.EventDetail)} property for details."
                            );
                        }

                        return shouldTrack;
                    }
                ).TakeUntil(taskInfo.TerminatingSequence)
                .ObserveOn(scheduler)
                .SubscribeSafe(this);
        }

        public void OnNext(IHyperNodeActivityEventItem activity)
        {
            try
            {
                _underlyingMonitor.OnTrack(activity);
            }
            catch (Exception ex)
            {
                // If the user-defined OnTrack() method throws an exception, kill the subscription immediately (and have our scheduler schedule itself for disposal) to prevent further exceptions from being thrown...
                _subscription?.Dispose();

                // Second, schedule the disposal of the scheduler itself if applicable
                if (_scheduler is IDisposable disposableScheduler)
                    _scheduler.Schedule(() => disposableScheduler.Dispose());

                // Tattle to everyone else and alert the other observers of what the original problem was
                _activity?.TrackException(
                    ex,
                    $"Activity monitor with {nameof(_underlyingMonitor.Name)} '{_underlyingMonitor.Name}' of type '{_underlyingMonitor.GetType().FullName}' threw an exception while attempting to track an activity event. The monitor has been unsubscribed and will not receive any additional notifications. See the {nameof(IActivityItem.EventDetail)} property for details."
                );
            }
        }

        public void OnError(Exception error)
        {
            try
            {
                _underlyingMonitor.OnActivityReportingError(error);
            }
            catch (Exception ex)
            {
                /*
                 * If we get here, the IObservable<T> threw an exception and notified every subscriber of the error.
                 * We tried to handle the exception in OnActivityReportingError(), but that threw an exception too.
                 * With the OnTrack() method, the consequence of such epic failure is to dispose of the faulty
                 * monitor and shame it by telling all the other monitors about the problem. However, in this case,
                 * all of the other monitors have already been notified of the exception and furthermore cannot be
                 * notified any further problems because the event sequence has been terminated. Therefore we are
                 * out of options and are forced to eat the exception. The best we can hope for is to log it, so
                 * we'll at least do that. In any case, hopefully one of the other monitors was able to handle the
                 * error successfully so that we have some chance of resolving the problem.
                 */
                _logger.LogError(
                    ex,
                    "Activity monitor with {nameProperty} '{nameValue}' of type '{typeName}' threw an exception while attempting to track an error thrown by the event stream. See the {eventDetail} property for details.",
                    nameof(_underlyingMonitor.Name),
                    _underlyingMonitor.Name,
                    _underlyingMonitor.GetType().FullName,
                    nameof(IActivityItem.EventDetail)
                );
            }
        }

        public void OnCompleted()
        {
            try
            {
                _underlyingMonitor.OnTaskCompleted();
            }
            catch (Exception ex)
            {
                /*
                 * If we get here, we end up in a similar situation compared to the OnError() delegate. Basically,
                 * we threw an exception after the task has completed and therefore we are unable to report it
                 * to anyone. We'll just log it and let that be the end of it.
                 */
                _logger.LogError(
                    ex,
                    "Activity monitor with {nameProperty} '{nameValue}' of type '{typeName}' threw an exception in response to the event stream's completion event. See the {eventDetail} property for details.",
                    nameof(_underlyingMonitor.Name),
                    _underlyingMonitor.Name,
                    _underlyingMonitor.GetType().FullName,
                    nameof(IActivityItem.EventDetail)
                );
            }
        }

        /// <summary>
        /// Immediately releases disposable resources consumed by this instance.
        /// Note that we are relying on Reactive Extensions to automatically dispose of subscriptions when the terminating sequence
        /// produces a value during the disposal of the <see cref="HyperNodeTaskInfo"/> instance. However, because it's good practice,
        /// I want to leave this <see cref="IDisposable"/> implementation in place so that if I need it in the future, it will be here.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                // First, dispose of our subscription, just in case
                _subscription?.Dispose();

                // Next, dispose of our scheduler, if applicable
                (_scheduler as IDisposable)?.Dispose();

                // Finally, make sure we don't schedule disposal more than once
                _isDisposed = true;
            }
        }
    }
}
