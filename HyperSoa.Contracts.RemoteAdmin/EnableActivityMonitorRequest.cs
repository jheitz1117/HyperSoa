using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.EnableActivityMonitor"/> system command.
    /// </summary>
    [ProtoContract]
    public class EnableActivityMonitorRequest : ICommandRequest
    {
        /// <summary>
        /// The name of the custom activity monitor to enable or disable.
        /// </summary>
        [ProtoMember(1)]
        public string ActivityMonitorName { get; set; }

        /// <summary>
        /// Indicates whether to enable or disable the custom activity monitor with the specified name.
        /// </summary>
        [ProtoMember(2)]
        public bool Enable { get; set; }
    }
}
