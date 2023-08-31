using HostingTest.Client.Serialization;
using HostingTest.Contracts;
using HyperSoa.Client;
using HyperSoa.Client.Extensions;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;

namespace HostingTest.Client
{
    public class HostingTestClient : OpinionatedHyperNodeClientBase
    {
        public HostingTestClient(IHyperNodeService underlyingService)
            : base(underlyingService) { }

        public HostingTestClient(string clientApplicationName, string endpoint)
            : base(new HyperNodeHttpClient(endpoint).From(clientApplicationName)) { }

        #region Public Methods

        public async Task<string> RunLongRunningCommandAsync(LongRunningCommandRequest request)
        {
            return await ProcessMessageAsync<LongRunningCommandRequest, EmptyCommandResponse>(
                "LongRunningCommand",
                request
            ).ConfigureAwait(false);
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ComplexCommandRequest request)
        {
            ComplexCommandResponse? commandResponse = null;

            await ProcessMessageAsync<ComplexCommandRequest, ComplexCommandResponse>(
                "ComplexCommand",
                request,
                r => commandResponse = r
            ).ConfigureAwait(false);

            return commandResponse ?? throw new InvalidOperationException("Unable to deserialize command response.");
        }

        public async Task EmptyContractCommandAsync()
        {
            await ProcessMessageAsync<EmptyCommandRequest, EmptyCommandResponse>(
                "EmptyContractCommand",
                null,
                _ => {}
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<string> ProcessMessageAsync<TRequest, TResponse>(string commandName, TRequest? request, Action<TResponse?>? onCommandResponse = null)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            var serializer = CreateSerializer<TRequest, TResponse>(commandName);

            Action<byte[]?>? onCommandResponseBytes = null;

            if (onCommandResponse != null)
            {
                onCommandResponseBytes = commandResponseBytes =>
                {
                    TResponse? commandResponse = default;

                    if (commandResponseBytes?.Length > 0)
                        commandResponse = serializer.DeserializeResponse(commandResponseBytes);
                        
                    onCommandResponse(commandResponse);
                };
            }

            return await base.ProcessMessageAsync(
                commandName,
                serializer.SerializeRequest(request),
                onCommandResponseBytes
            ).ConfigureAwait(false);
        }

        private static IClientContractSerializer<TRequest, TResponse> CreateSerializer<TRequest, TResponse>(string commandName)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            IClientContractSerializer<TRequest, TResponse> serializer;

            switch (commandName)
            {
                case "ComplexCommand":
                    serializer = new DataContractJsonSerializer<TRequest, TResponse>();
                    break;
                default:
                    serializer = new ProtoContractSerializer<TRequest, TResponse>();
                    break;
            }

            return serializer;
        }
        
        #endregion Private Methods
    }
}