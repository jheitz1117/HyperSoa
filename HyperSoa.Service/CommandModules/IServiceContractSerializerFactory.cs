using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules
{
    /// <summary>
    /// Defines a method to create instances of <see cref="IServiceContractSerializer"/>.
    /// </summary>
    public interface IServiceContractSerializerFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IServiceContractSerializer"/>.
        /// </summary>
        /// <returns></returns>
        IServiceContractSerializer Create();
    }
}
