using HyperSoa.Contracts;
using HyperSoa.Service.ActivityTracking;

namespace HyperSoa.Service.CommandModules
{
    /// <summary>
    /// Defines properties describing the environment in which to execute an <see cref="ICommandModule"/>.
    /// </summary>
    public interface ICommandExecutionContext : IReadOnlyHyperNodeMessageInfo
    {
        /// <summary>
        /// The ID of the current task.
        /// </summary>
        string TaskId { get; }

        /// <summary>
        /// The name of the <see cref="IHyperNodeService"/> running the current task.
        /// </summary>
        string ExecutingNodeName { get; }

        /// <summary>
        /// The request object containing parameters to be passed to the <see cref="ICommandModule"/>.
        /// </summary>
        ICommandRequest? Request { get; }

        /// <summary>
        /// Provides a way to raise activity events from inside an <see cref="ICommandModule"/>.
        /// </summary>
        ITaskActivityTracker Activity { get; }

        /// <summary>
        /// Provides a way for the <see cref="ICommandModule"/> to terminate cooperatively when cancellation has been requested.
        /// </summary>
        CancellationToken Token { get; }
    }
}
