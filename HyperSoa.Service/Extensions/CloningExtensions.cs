using HyperSoa.Contracts;

namespace HyperSoa.Service.Extensions
{
    internal static class CloningExtensions
    {
        public static HyperNodeActivityItem Clone(this HyperNodeActivityItem source)
        {
            return new HyperNodeActivityItem
            {
                Agent = source.Agent,
                Elapsed = source.Elapsed,
                EventDateTime = source.EventDateTime,
                EventDescription = source.EventDescription,
                EventDetail = source.EventDetail,
                ProgressPart = source.ProgressPart,
                ProgressTotal = source.ProgressTotal
            };
        }
    }
}
