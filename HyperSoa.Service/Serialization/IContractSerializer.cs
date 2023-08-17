using HyperSoa.Contracts;

namespace HyperSoa.Service.Serialization
{
    /// <summary>
    /// Provides methods to deserialize <see cref="ICommandRequest"/> objects and
    /// serialize <see cref="ICommandResponse"/> objects.
    /// </summary>
    public interface IContractSerializer
    {
        /// <summary>
        /// Deserializes the specified <paramref name="requestBytes"/> and returns an <see cref="ICommandRequest"/> object.
        /// </summary>
        /// <param name="requestBytes">The byte array containing the object data to deserialize.</param>
        /// <returns></returns>
        ICommandRequest? DeserializeRequest(byte[]? requestBytes);

        /// <summary>
        /// Serializes the specified <see cref="ICommandResponse"/> object into a byte array.
        /// </summary>
        /// <param name="response">The <see cref="ICommandResponse"/> object to serialize.</param>
        /// <returns></returns>
        byte[]? SerializeResponse(ICommandResponse? response);

        /// <summary>
        /// Gets the expected <see cref="Type"/> of the <see cref="ICommandRequest"/> object.
        /// </summary>
        /// <returns></returns>
        Type GetRequestType();
    }
}
