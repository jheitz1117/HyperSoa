using HyperSoa.Contracts;

namespace HyperSoa.Client.Serialization
{
    /// <summary>
    /// Provides methods to serialize <see cref="ICommandRequest"/> objects and
    /// deserialize <see cref="ICommandResponse"/> objects.
    /// </summary>
    public interface IContractSerializer
    {
        /// <summary>
        /// Serializes the specified <see cref="ICommandRequest"/> object into a byte array.
        /// </summary>
        /// <param name="request">The <see cref="ICommandRequest"/> object to serialize.</param>
        /// <returns></returns>
        byte[]? SerializeRequest(ICommandRequest? request);

        /// <summary>
        /// Deserializes the specified <paramref name="responseBytes"/> and returns an <see cref="ICommandResponse"/> object.
        /// </summary>
        /// <param name="responseBytes">The byte array containing the object data to deserialize.</param>
        /// <returns></returns>
        ICommandResponse? DeserializeResponse(byte[]? responseBytes);
    }
}
