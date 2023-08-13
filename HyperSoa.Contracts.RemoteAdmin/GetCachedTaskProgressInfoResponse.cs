using HyperSoa.Contracts.RemoteAdmin.Models;
using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandResponse"/> for the <see cref="RemoteAdminCommandName.GetCachedTaskProgressInfo"/> system command.
    /// </summary>
    [ProtoContract]
    public class GetCachedTaskProgressInfoResponse : ICommandResponse
    {
        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }

        /// <summary>
        /// Indicates whether the task progress cache is enabled for the <see cref="IHyperNodeService"/>.
        /// </summary>
        [ProtoMember(2)]
        public bool TaskProgressCacheEnabled { get; set; }

        /// <summary>
        /// If the cache is enabled, contains the progress of the task with the requested ID.
        /// </summary>
        [ProtoMember(3)]
        public HyperNodeTaskProgressInfo? TaskProgressInfo { get; set; }
    }
}
