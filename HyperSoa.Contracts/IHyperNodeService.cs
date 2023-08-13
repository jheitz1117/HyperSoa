namespace HyperSoa.Contracts
{
    public interface IHyperNodeService
    {
        /// <summary>
        /// Processes and/or forwards the specified message.
        /// </summary>
        /// <param name="message">The <see cref="HyperNodeMessageRequest"/> object to process.</param>
        /// <returns></returns>
        Task<HyperNodeMessageResponse> ProcessMessageAsync(HyperNodeMessageRequest message);
    }
}