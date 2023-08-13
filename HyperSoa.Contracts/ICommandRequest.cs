using HyperSoa.Contracts.Serialization;

namespace HyperSoa.Contracts
{
    /// <summary>
    /// Marks a class as a request object for a custom command module. Classes implementing this interface can be serialized
    /// and deserialized using an instance of <see cref="IContractSerializer"/>.
    /// </summary>
    public interface ICommandRequest { }
}
