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
            : base(new HyperNodeHttpClient(endpoint))
        {
            ClientApplicationName = clientApplicationName;
        }

        #region Public Methods

        public async Task<string> RunLongRunningCommandAsync(LongRunningCommandRequest request)
        {
            return await RunLongRunningCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> RunLongRunningCommandAsync(ICommandMetaData<LongRunningCommandRequest> request)
        {
            return await ProcessMessageAsync<LongRunningCommandRequest, EmptyCommandResponse>(
                "LongRunningCommand",
                request.WithSerializer(
                    new DataContractJsonSerializer<LongRunningCommandRequest, EmptyCommandResponse>()
                )
            ).ConfigureAwait(false);
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ComplexCommandRequest request)
        {
            return await ComplexCommandAsync(
                request.CreatedBy(ClientApplicationName)
            );
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ICommandMetaData<ComplexCommandRequest> request)
        {
            ComplexCommandResponse? commandResponse = null;

            await ProcessMessageAsync<ComplexCommandRequest, ComplexCommandResponse>(
                "ComplexCommand",
                request.WithSerializer(
                    new DataContractJsonSerializer<ComplexCommandRequest, ComplexCommandResponse>()
                ),
                r => commandResponse = r
            ).ConfigureAwait(false);

            return commandResponse ?? throw new InvalidOperationException("Unable to deserialize command response.");
        }

        public async Task EmptyContractCommandAsync()
        {
            await EmptyContractCommandAsync(
                new EmptyCommandRequest().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task EmptyContractCommandAsync(ICommandMetaData<EmptyCommandRequest> request)
        {
            await ProcessMessageAsync<EmptyCommandRequest, EmptyCommandResponse>(
                "EmptyCommand",
                request,
                _ => {}
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<string> ProcessMessageAsync<TRequest, TResponse>(string commandName, ICommandMetaData<TRequest>? metaData, Action<TResponse?>? onCommandResponse = null)
            where TRequest : ICommandRequest
            where TResponse : ICommandResponse
        {
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();
            
            Action<byte[]?>? onCommandResponseBytes = null;

            if (onCommandResponse != null)
            {
                onCommandResponseBytes = commandResponseBytes =>
                {
                    TResponse? commandResponse = default;

                    if (commandResponseBytes?.Length > 0 && metaData?.Serializer != null)
                        commandResponse = (TResponse?)metaData.Serializer.DeserializeResponse(commandResponseBytes);
                        
                    onCommandResponse(commandResponse);
                };
            }

            return await base.ProcessMessageAsync(
                commandName,
                metaData,
                onCommandResponseBytes
            ).ConfigureAwait(false);
        }
        
        #endregion Private Methods
    }
}