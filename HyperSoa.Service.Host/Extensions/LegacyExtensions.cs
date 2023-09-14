using System.Text;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Legacy;

namespace HyperSoa.Service.Host.Extensions
{
    internal static class LegacyExtensions
    {
        public static HyperNodeMessageRequest ToHyperNodeMessageRequest(this LegacyHyperNodeMessageRequest legacyRequest)
        {
            return new HyperNodeMessageRequest
            {
                CommandName = legacyRequest.CommandName,
                CommandRequestBytes = legacyRequest.CommandRequestString != null
                    ? Convert.FromBase64String(legacyRequest.CommandRequestString)
                    : null,
                CreatedByAgentName = legacyRequest.CreatedByAgentName,
                ProcessOptionFlags = legacyRequest.ProcessOptionFlags
            };
        }

        public static LegacyHyperNodeMessageResponse ToLegacyHyperNodeMessageResponse(this HyperNodeMessageResponse response)
        {
            return new LegacyHyperNodeMessageResponse
            {
                TaskId = response.TaskId,
                RespondingNodeName = response.RespondingNodeName,
                TotalRunTime = response.TotalRunTime,
                NodeAction = response.NodeAction,
                NodeActionReason = response.NodeActionReason,
                ProcessStatusFlags = response.ProcessStatusFlags,
                TaskTrace = new List<HyperNodeActivityItem>(
                    response.TaskTrace ?? Array.Empty<HyperNodeActivityItem>()
                ),
                CommandResponseString = response.CommandResponseBytes != null
                    ? Convert.ToBase64String(response.CommandResponseBytes)
                    : null,
            };
        }
    }
}
