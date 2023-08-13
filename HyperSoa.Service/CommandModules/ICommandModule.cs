using HyperSoa.Contracts;

namespace HyperSoa.Service.CommandModules
{
    /// <summary>
    /// Defines a custom command module for an <see cref="IHyperNodeService"/>.
    /// </summary>
    public interface ICommandModule
    {
        /// <summary>
        /// Executes a user-defined code block using the specified <see cref="ICommandExecutionContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="ICommandExecutionContext"/> containing contextual information about the execution environment.</param>
        /// <returns></returns>
        ICommandResponse Execute(ICommandExecutionContext context);
    }
}
