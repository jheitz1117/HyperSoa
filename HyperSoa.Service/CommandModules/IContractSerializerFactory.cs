using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules
{
    /// <summary>
    /// Defines a method to create instances of <see cref="IContractSerializer"/>.
    /// </summary>
    public interface IContractSerializerFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IContractSerializer"/>.
        /// </summary>
        /// <returns></returns>
        IContractSerializer Create();
    }
}
