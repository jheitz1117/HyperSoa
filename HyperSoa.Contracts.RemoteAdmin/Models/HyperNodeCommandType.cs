using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin.Models
{
    /// <summary>
    /// Command types for command modules.
    /// </summary>
    [ProtoContract]
    public enum HyperNodeCommandType
    {
        /// <summary>
        /// Indicates that the command is of unknown origin.
        /// </summary>
        [ProtoEnum]
        Unknown = 0,

        /// <summary>
        /// Indicates that the command is a remote administration command.
        /// </summary>
        [ProtoEnum]
        RemoteAdmin = 1,

        /// <summary>
        /// Indicates that the command is a user-defined command.
        /// </summary>
        [ProtoEnum]
        Custom = 2
    }
}
