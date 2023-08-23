using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.EnableCommand"/> system command.
    /// </summary>
    [ProtoContract]
    public class EnableCommandModuleRequest : ICommandRequest
    {
        /// <summary>
        /// The name of the command to enable or disable.
        /// </summary>
        [ProtoMember(1)]
        public string? CommandName { get; set; }

        /// <summary>
        /// Indicates whether to enable or disable the command with the specified command name.
        /// </summary>
        [ProtoMember(2)]
        public bool Enable { get; set; }
    }
}
