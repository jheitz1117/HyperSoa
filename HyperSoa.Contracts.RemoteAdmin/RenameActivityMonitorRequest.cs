using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.RenameActivityMonitor"/> system command.
    /// </summary>
    [ProtoContract]
    public class RenameActivityMonitorRequest : ICommandRequest
    {
        /// <summary>
        /// The name of the custom activity monitor to rename.
        /// </summary>
        [ProtoMember(1)]
        public string OldName { get; set; }

        /// <summary>
        /// The new name of the custom activity monitor.
        /// </summary>
        [ProtoMember(2)]
        public string NewName { get; set; }
    }
}
