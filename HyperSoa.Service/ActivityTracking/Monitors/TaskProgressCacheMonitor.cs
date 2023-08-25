using System.Runtime.Caching;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin.Models;

namespace HyperSoa.Service.ActivityTracking.Monitors
{
    /// <summary>
    /// Collects <see cref="HyperNodeTaskProgressInfo"/> objects based on task ID.
    /// </summary>
    internal sealed class TaskProgressCacheMonitor : HyperNodeServiceActivityMonitor, IDisposable
    {
        private static readonly object Lock = new();
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private static readonly CacheItemPolicy CachePolicy = new();

        public static TimeSpan CacheDuration
        {
            get => CachePolicy.SlidingExpiration;
            set => CachePolicy.SlidingExpiration = value;
        }

        public TaskProgressCacheMonitor()
        {
            Name = nameof(TaskProgressCacheMonitor);
        }

        public static void ResetTaskIdProgress(string? taskId)
        {
            if (!string.IsNullOrWhiteSpace(taskId))
                Cache.Remove(taskId);
        }

        public override void OnTrack(IHyperNodeActivityEventItem activity)
        {
            // Ignore all activity that doesn't have a task ID
            if (string.IsNullOrWhiteSpace(activity.TaskId))
                return;

            // First add a new cache item or get the existing cache item with the specified key
            var taskProgressInfo = AddOrGetExisting(activity.TaskId, () => new CachedTaskProgressInfo());

            // Now add our specific item to our list of activity items. Need lock because list is not thread-safe
            lock (Lock)
            {
                taskProgressInfo.Activity.Add(
                    new HyperNodeActivityItem
                    {
                        Agent = activity.Agent,
                        EventDateTime = activity.EventDateTime,
                        EventDescription = activity.EventDescription,
                        EventDetail = activity.EventDetail,
                        ProgressPart = activity.ProgressPart,
                        ProgressTotal = activity.ProgressTotal,
                        Elapsed = activity.Elapsed
                    }
                );
            }

            // Check if we were given a response to cache
            if (activity.EventData is HyperNodeMessageResponse response)
            {
                taskProgressInfo.IsComplete = true;
                taskProgressInfo.Response = response;
            }

            // Make sure our progress properties are updated. If no values were supplied, then we presume there are no updates, but we'll keep any values we had before
            taskProgressInfo.ProgressPart = activity.ProgressPart ?? taskProgressInfo.ProgressPart;
            taskProgressInfo.ProgressTotal = activity.ProgressTotal ?? taskProgressInfo.ProgressTotal;
        }

        public override void OnActivityReportingError(Exception exception)
        {
            // First add a new cache item or get the existing cache item with the specified key
            var taskProgressInfo = AddOrGetExisting(nameof(TaskProgressCacheMonitor), () => new CachedTaskProgressInfo());

            // Now add our specific item to our list of activity items. Need lock because list is not thread-safe
            lock (Lock)
            {
                taskProgressInfo.Activity.Add(
                    new HyperNodeActivityItem
                    {
                        Agent = nameof(TaskProgressCacheMonitor),
                        EventDateTime = DateTime.Now,
                        EventDescription = exception.Message,
                        EventDetail = exception.ToString()
                    }
                );
            }
        }

        /// <summary>
        /// Gets the <see cref="HyperNodeTaskProgressInfo"/> object from the cache with the specified task ID. If no cache item exists with the specified task ID, return null.
        /// </summary>
        /// <param name="taskId">The task ID to look for.</param>
        /// <returns></returns>
        public static HyperNodeTaskProgressInfo? GetTaskProgressInfo(string taskId)
        {
            HyperNodeTaskProgressInfo? snapshot = null;

            var cachedProgress = (
                Cache[taskId] as Lazy<CachedTaskProgressInfo?> ?? new Lazy<CachedTaskProgressInfo?>(
                    () => null // If we don't have any task progress for the specified task ID, just return null
                )
            ).Value;

            if (cachedProgress != null)
            {
                // Need this lock to synchronize access to the activity list, which could be modified during enumeration
                lock (Lock)
                {
                    snapshot = new HyperNodeTaskProgressInfo
                    {
                        ProgressTotal = cachedProgress.ProgressTotal,
                        ProgressPart = cachedProgress.ProgressPart,
                        Activity = cachedProgress.Activity.ToArray(),
                        IsComplete = cachedProgress.IsComplete,
                        Response = cachedProgress.Response,
                    };
                }
            }

            return snapshot;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #region Private Methods

        /// <summary>
        /// This method wraps the <see cref="MemoryCache"/> object's AddOrGetExisting() method. The need for the wrapper is obscure and only becomes evident upon close examination.
        /// It turns out that <see cref="MemoryCache"/>'s AddOrGetExisting() method does not return the new value you're giving it, but rather the old value that was there before.
        /// For new inserts, this old value is considered to be null, so you get back a null value instead of the value you tried to insert as you might have expected. This wrapper
        /// accounts for that small caveat by utilizing .NET's <see cref="Lazy&lt;T&gt;"/> class.
        /// 
        /// This wrapper method was taken from a blog at https://medium.com/falafel-software/working-with-system-runtime-caching-memorycache-9f8548172ccd.
        /// </summary>
        /// <typeparam name="T">Type of object being stored in the <see cref="MemoryCache"/></typeparam>
        /// <param name="key">A unique identifier for the cache entry to add or get.</param>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
        /// <returns></returns>
        private static T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = Cache.AddOrGetExisting(key, newValue, CachePolicy) as Lazy<T>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache
                Cache.Remove(key);
                throw;
            }
        }

        private static void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cache.Dispose();
            }
        }

        #endregion Private Methods
    }
}
