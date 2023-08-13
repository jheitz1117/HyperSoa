using HyperSoa.Contracts.RemoteAdmin.Models;
using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandResponse"/> for the <see cref="RemoteAdminCommandName.GetNodeStatus"/> system command.
    /// </summary>
    [ProtoContract]
    public class GetNodeStatusResponse : ICommandResponse
    {
        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }

        /// <summary>
        /// The sliding expiration of items in the task progress cache.
        /// </summary>
        [ProtoMember(2)]
        public TimeSpan TaskProgressCacheDuration { get; set; }

        /// <summary>
        /// Indicates whether the task progress cache is enabled or disabled.
        /// </summary>
        [ProtoMember(3)]
        public bool TaskProgressCacheEnabled { get; set; }

        /// <summary>
        /// Indicates whether diagnostics are enabled or disabled.
        /// </summary>
        [ProtoMember(4)]
        public bool DiagnosticsEnabled { get; set; }

        /// <summary>
        /// The maximum number of tasks that are allowed to run concurrently.
        /// </summary>
        [ProtoMember(5)]
        public int MaxConcurrentTasks { get; set; }

        /// <summary>
        /// A list of <see cref="CommandStatus"/> objects reporting the status of each command module in the <see cref="IHyperNodeService"/>.
        /// </summary>
        [ProtoMember(6)]
        public IEnumerable<CommandStatus> Commands { get; set; } = new List<CommandStatus>();

        /// <summary>
        /// A list of <see cref="ActivityMonitorStatus"/> objects reporting the status of each custom activity monitor in the <see cref="IHyperNodeService"/>.
        /// </summary>
        [ProtoMember(7)]
        public IEnumerable<ActivityMonitorStatus> ActivityMonitors { get; set; } = new List<ActivityMonitorStatus>();

        ///// <summary>
        ///// A list of <see cref="LiveTaskStatus"/> objects reporting the status of each live task in the <see cref="IHyperNodeService"/>.
        ///// </summary>
        [ProtoMember(8)]
        public IEnumerable<LiveTaskStatus> LiveTasks { get; set; } = new List<LiveTaskStatus>();
    }
}
