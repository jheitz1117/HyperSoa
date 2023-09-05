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
            return await RunCommandAsync(
                "LongRunningCommand",
                request
            ).ConfigureAwait(false);
        }

        public async Task<string> RunLongRunningSingletonCommandAsync(LongRunningCommandRequest request)
        {
            return await RunLongRunningSingletonCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> RunLongRunningSingletonCommandAsync(ICommandMetaData<LongRunningCommandRequest> request)
        {
            return await RunCommandAsync(
                "LongRunningSingletonCommand",
                request
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
            return await GetCommandResponseAsync<ComplexCommandRequest, ComplexCommandResponse>(
                "ComplexCommand",
                request.WithSerializer(
                    new DataContractJsonSerializer<ComplexCommandRequest, ComplexCommandResponse>()
                )
            ).ConfigureAwait(false);
        }

        public async Task EmptyContractCommandAsync()
        {
            await EmptyContractCommandAsync(
                new EmptyCommandRequest().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task EmptyContractCommandAsync(ICommandMetaData<EmptyCommandRequest> request)
        {
            await GetCommandResponseAsync<EmptyCommandRequest, EmptyCommandResponse>(
                "EmptyCommand",
                request
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData<TRequest>? metaData)
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();

            return base.GetCommandResponseAsync<TRequest, TResponse>(commandName, metaData);
        }

        protected Task<string> RunCommandAsync<TRequest>(string commandName, ICommandMetaData<TRequest>? metaData)
            where TRequest : ICommandRequest
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, ICommandResponse>();

            return base.RunCommandAsync(commandName, metaData);
        }

        #endregion Protected Methods
    }
}