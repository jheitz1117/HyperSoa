using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.EnableDiagnostics"/> system command.
    /// </summary>
    [ProtoContract]
    public class EnableDiagnosticsRequest : ICommandRequest
    {
        /// <summary>
        /// Indicates whether to enable or disable diagnostics.
        /// </summary>
        [ProtoMember(1)]
        public bool Enable { get; set; }
    }
}
