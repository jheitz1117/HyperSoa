using HyperSoa.Contracts;

namespace HyperSoa.Service.Extensions
{
    internal static class ContractExtensions
    {
        public static bool TaskTraceRequested(this HyperNodeMessageRequest request)
        {
            return request.ProcessOptionFlags.HasFlag(MessageProcessOptionFlags.ReturnTaskTrace);
        }

        public static bool ConcurrentRunRequested(this HyperNodeMessageRequest request)
        {
            return request.ProcessOptionFlags.HasFlag(MessageProcessOptionFlags.RunConcurrently);
        }

        public static bool ProgressCacheRequested(this HyperNodeMessageRequest request)
        {
            return request.ProcessOptionFlags.HasFlag(MessageProcessOptionFlags.CacheTaskProgress);
        }
    }
}
