using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;

namespace HyperSoa.Service.Configuration
{
    /// <summary>
    /// Defines configurable properties of a collection of system-level <see cref="ICommandModule"/> objects in an <see cref="IHyperNodeService"/>.
    /// </summary>
    public interface IRemoteAdminConfigurationCollection : IEnumerable<IRemoteAdminCommandConfiguration>
    {
        /// <summary>
        /// Indicates whether system-level <see cref="ICommandModule"/> objects in the <see cref="IHyperNodeService"/> will be enabled
        /// when the <see cref="IHyperNodeService"/> starts. This property can be overridden for a specific command by adding an
        /// instance of <see cref="IRemoteAdminCommandConfiguration"/> to the <see cref="IRemoteAdminConfigurationCollection"/> with
        /// the desired configuration.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets the <see cref="IRemoteAdminCommandConfiguration"/> object with the specified name.
        /// </summary>
        /// <param name="commandName">The name of the <see cref="IRemoteAdminCommandConfiguration"/> object to get.</param>
        /// <returns></returns>
        IRemoteAdminCommandConfiguration? GetByCommandName(string commandName);

        /// <summary>
        /// Determines whether the <see cref="IRemoteAdminConfigurationCollection"/> contains an instance of <see cref="IRemoteAdminCommandConfiguration"/> with the specified name.
        /// </summary>
        /// <param name="commandName">The name of the <see cref="IRemoteAdminCommandConfiguration"/> object to locate in the <see cref="IRemoteAdminConfigurationCollection"/>.</param>
        /// <returns></returns>
        bool ContainsCommandName(string commandName);
    }
}
