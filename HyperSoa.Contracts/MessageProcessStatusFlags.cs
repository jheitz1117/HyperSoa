using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Flags that indicate what happened while a command was running in a <see cref="IHyperNodeService"/>.
    /// </summary>
    [Flags]
    [ProtoContract]
    public enum MessageProcessStatusFlags
    {
        /// <summary>
        /// Indicates that the message was not processed
        /// </summary>
        [ProtoEnum]
        None                  = 0,

        /// <summary>
        /// Indicates that the message was processed successfully.
        /// </summary>
        [ProtoEnum]
        Success               = 1 << 0,

        /// <summary>
        /// Indicates that the message could not be processed.
        /// </summary>
        [ProtoEnum]
        Failure               = 1 << 1,

        /// <summary>
        /// Indicates that the message contained an invalid command name.
        /// </summary>
        [ProtoEnum]
        InvalidCommand        = 1 << 2,

        /// <summary>
        /// Indicates that some non-fatal errors occurred while processing the message.
        /// </summary>
        [ProtoEnum]
        HadNonFatalErrors     = 1 << 3,

        /// <summary>
        /// Indicates that some warnings occurred while processing the message.
        /// </summary>
        [ProtoEnum]
        HadWarnings           = 1 << 4,

        /// <summary>
        /// Indicates that the service was cancelled before it completed.
        /// </summary>
        [ProtoEnum]
        Cancelled             = 1 << 5,

        /// <summary>
        /// Indicates that the ICommandRequest implementation was invalid.
        /// </summary>
        [ProtoEnum]
        InvalidCommandRequest = 1 << 6
    }
}
