using ProtoBuf;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// The reasons why the <see cref="IHyperNodeService"/> chose to take the <see cref="HyperNodeActionType"/> specified in the <see cref="HyperNodeMessageResponse"/>.
    /// </summary>
    [ProtoContract]
    public enum HyperNodeActionReasonType
    {
        /// <summary>
        /// Indicates that no reason was specified for the action taken by the receiving <see cref="IHyperNodeService"/>.
        /// </summary>
        [ProtoEnum]
        Unknown = 0,

        /// <summary>
        /// Indicates that the message was rejected by user-defined code for a user-defined reason.
        /// </summary>
        [ProtoEnum]
        Custom = 1,

        /// <summary>
        /// Indicates that the message was valid and this <see cref="IHyperNodeService"/> is ready to process it.
        /// </summary>
        [ProtoEnum]
        ValidMessage = 5,

        /// <summary>
        /// Indicates that the receiving <see cref="IHyperNodeService"/> generated a task ID for a task that was already running.
        /// </summary>
        [ProtoEnum]
        DuplicateTaskId = 6,

        /// <summary>
        /// Indicates that the receiving <see cref="IHyperNodeService"/> has already reached its maximum number of concurrent tasks.
        /// </summary>
        [ProtoEnum]
        MaxConcurrentTaskCountReached = 7,

        /// <summary>
        /// Indicates that the Cancel() method has been called on the receiving <see cref="IHyperNodeService"/> and no new tasks are being started.
        /// </summary>
        [ProtoEnum]
        CancellationRequested = 8,

        /// <summary>
        /// Indicates that the ITaskIdProvider implementation threw an exception while generating a task ID.
        /// </summary>
        [ProtoEnum]
        TaskIdProviderThrewException = 9,

        /// <summary>
        /// Indicates that the ITaskIdProvider implementation generated an invalid task ID.
        /// </summary>
        [ProtoEnum]
        InvalidTaskId = 10,

        /// <summary>
        /// Indicates that an exception was thrown while attempting to communicate over the network.
        /// </summary>
        [ProtoEnum]
        CommunicationException = 11
    }
}
