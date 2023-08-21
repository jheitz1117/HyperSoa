﻿using System.Text;
using HyperSoa.Contracts;
using HyperSoa.ServiceHosting.Interop;

namespace HyperSoa.ServiceHosting.Extensions
{
    internal static class LegacyExtensions
    {
        public static HyperNodeMessageRequest ToHyperNodeMessageRequest(this LegacyHyperNodeMessageRequest legacyRequest)
        {
            return new HyperNodeMessageRequest
            {
                CommandName = legacyRequest.CommandName,
                CommandRequestBytes = legacyRequest.CommandRequestString != null
                    ? Encoding.UTF8.GetBytes(legacyRequest.CommandRequestString)
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
                    ? Encoding.UTF8.GetString(response.CommandResponseBytes)
                    : null,
            };
        }
    }
}
