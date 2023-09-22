using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public interface IOpinionatedHyperNodeClient : IHyperNodeService
    {
        public string? ClientApplicationName { get; set; }

        public Task<string> ExecuteCommandRemoteAsync(string commandName, ICommandMetaData? metaData = null);
        public Task ExecuteCommandAsync(string commandName, ICommandMetaData? metaData = null);
        public Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse;
    }
}
