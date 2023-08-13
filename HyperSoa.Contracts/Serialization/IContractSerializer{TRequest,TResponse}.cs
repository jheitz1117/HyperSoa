﻿namespace HyperSoa.Contracts.Serialization
{
    /// <summary>
    /// Provides methods to serialize and deserialize <see cref="ICommandRequest"/> objects.
    /// </summary>
    public interface IContractSerializer<out TRequest, in TResponse> : IContractSerializer
        where TRequest : ICommandRequest
        where TResponse : ICommandResponse
    {
        /// <summary>
        /// Deserializes the specified <paramref name="requestBytes"/> and returns an <see cref="ICommandRequest"/> object.
        /// </summary>
        /// <param name="requestBytes">The byte array containing the object data to deserialize.</param>
        /// <returns></returns>
        new TRequest? DeserializeRequest(byte[]? requestBytes);

        /// <summary>
        /// Serializes the specified <see cref="ICommandResponse"/> object into a byte array.
        /// </summary>
        /// <param name="response">The <see cref="ICommandResponse"/> object to serialize.</param>
        /// <returns></returns>
        byte[]? SerializeResponse(TResponse? response);
    }
}
