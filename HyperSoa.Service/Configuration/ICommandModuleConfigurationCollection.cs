using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.Configuration
{
    /// <summary>
    /// Defines configurable properties of a collection of user-defined <see cref="ICommandModule"/> objects in an <see cref="IHyperNodeService"/>.
    /// </summary>
    public interface ICommandModuleConfigurationCollection : IEnumerable<ICommandModuleConfiguration>
    {
        /// <summary>
        /// The assembly qualified name of a type that implements <see cref="IContractSerializer"/>. If this property is supplied,
        /// the specified type serves as the default <see cref="IContractSerializer{TRequest,TResponse}"/> implementation for all user-defined
        /// <see cref="ICommandModule"/> objects in the <see cref="ICommandModuleConfigurationCollection"/>. This collection-level
        /// default can be overridden for a specific command by supplying the desired <see cref="IContractSerializer"/> type
        /// string in the <see cref="ICommandModuleConfiguration.ContractSerializerType"/> property for the <see cref="ICommandModuleConfiguration"/>
        /// for that command.
        /// </summary>
        string? ContractSerializerType { get; }
    }
}
