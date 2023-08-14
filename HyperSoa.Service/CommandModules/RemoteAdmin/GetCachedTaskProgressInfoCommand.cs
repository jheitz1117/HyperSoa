using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.RemoteAdmin.Models;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules.RemoteAdmin
{
    internal class GetCachedTaskProgressInfoCommand : ICommandModule, IContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            if (context.Request is not GetCachedTaskProgressInfoRequest request)
                throw new InvalidCommandRequestTypeException(typeof(GetCachedTaskProgressInfoRequest), context.Request.GetType());

            var response = new GetCachedTaskProgressInfoResponse
            {
                TaskProgressCacheEnabled = HyperNodeService.Instance.EnableTaskProgressCache
            };

            if (!response.TaskProgressCacheEnabled)
            {
                context.Activity.Track("Warning: The task progress cache is disabled.");
                response.ProcessStatusFlags |= MessageProcessStatusFlags.HadWarnings;
            }

            context.Activity.Track($"Retrieving cached task progress info for Task ID '{request.TaskId}'.");
            response.TaskProgressInfo = HyperNodeService.Instance.GetCachedTaskProgressInfo(request.TaskId);

            // If we can't find any task progress info for the specified Task ID, we'll return a placeholder object in Completed status that informs the caller that no progress
            // information exists for this task ID. This will prevent the caller from sitting in an infinite loop waiting for IsComplete to be true when there may not be a cache
            // item available, or no cache at all
            response.TaskProgressInfo ??= new HyperNodeTaskProgressInfo
            {
                IsComplete = true,
                Activity = new List<HyperNodeActivityItem>
                {
                    new()
                    {
                        EventDateTime = DateTime.Now,
                        EventDescription = $"No task progress information exists for Task ID '{request.TaskId}'.",
                        Agent = context.ExecutingNodeName
                    }
                }
            };

            response.ProcessStatusFlags |= MessageProcessStatusFlags.Success;

            return response;
        }

        public IContractSerializer Create()
        {
            return new ProtoContractSerializer<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>();
        }
    }
}
