using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin.Models
{
    /// <summary>
    /// Describes the status of a command module.
    /// </summary>
    [ProtoContract]
    public class CommandStatus
    {
        /// <summary>
        /// The command name of the module.
        /// </summary>
        [ProtoMember(1)]
        public string CommandName { get; set; }
        
        /// <summary>
        /// Indicates whether the command module is enabled or disabled.
        /// </summary>
        [ProtoMember(2)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates whether the command module is a <see cref="HyperNodeCommandType.RemoteAdmin"/> or a <see cref="HyperNodeCommandType.Custom"/>.
        /// </summary>
        [ProtoMember(3)]
        public HyperNodeCommandType CommandType { get; set; }
    }
}
