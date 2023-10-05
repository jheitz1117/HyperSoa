
namespace HyperSoa.Contracts.Legacy.Extensions
{
    public static class LegacyExtensions
    {
        public static LegacyHyperNodeMessageRequest ToLegacyHyperNodeMessageRequest(this HyperNodeMessageRequest request) {
            return new LegacyHyperNodeMessageRequest {
                CommandName = request.CommandName,
                CommandRequestString = request.CommandRequestBytes == null 
                    ? null 
                    : Convert.ToBase64String(request.CommandRequestBytes),
                CreatedByAgentName = request.CreatedByAgentName,
                ProcessOptionFlags = request.ProcessOptionFlags
            };
        }
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

        public static HyperNodeMessageResponse ToHyperNodeMessageResponse(this LegacyHyperNodeMessageResponse legacyResponse) {
            return new HyperNodeMessageResponse {
                TaskId = legacyResponse.TaskId,
                RespondingNodeName = legacyResponse.RespondingNodeName,
                TotalRunTime = legacyResponse.TotalRunTime,
                NodeAction = legacyResponse.NodeAction,
                NodeActionReason = legacyResponse.NodeActionReason,
                ProcessStatusFlags = legacyResponse.ProcessStatusFlags,
                TaskTrace = legacyResponse.TaskTrace?.ToArray(),
                CommandResponseBytes = legacyResponse.CommandResponseString != null
                    ? Convert.FromBase64String(legacyResponse.CommandResponseString)
                    : null
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
