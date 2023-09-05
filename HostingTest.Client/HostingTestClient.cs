using HostingTest.Client.Extensions;
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

        public async Task<string> RunLongRunningCommandAsync(ICommandMetaData metaData)
        {
            return await RunCommandAsync(
                "LongRunningCommand",
                metaData.WithDefaultSerializer<LongRunningCommandRequest, ICommandResponse>()
            ).ConfigureAwait(false);
        }

        public async Task<string> RunLongRunningSingletonCommandAsync(LongRunningCommandRequest request)
        {
            return await RunLongRunningSingletonCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> RunLongRunningSingletonCommandAsync(ICommandMetaData metaData)
        {
            return await RunCommandAsync(
                "LongRunningSingletonCommand",
                metaData.WithDefaultSerializer<LongRunningCommandRequest, ICommandResponse>()
            ).ConfigureAwait(false);
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ComplexCommandRequest request)
        {
            return await ComplexCommandAsync(
                request.CreatedBy(ClientApplicationName)
            );
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ICommandMetaData metaData)
        {
            return await GetCommandResponseAsync<ComplexCommandRequest, ComplexCommandResponse>(
                "ComplexCommand",
                metaData.WithSerializer(
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

        public async Task EmptyContractCommandAsync(ICommandMetaData metaData)
        {
            await GetCommandResponseAsync<EmptyCommandRequest, EmptyCommandResponse>(
                "EmptyCommand",
                metaData
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();

            return base.GetCommandResponseAsync<TRequest, TResponse>(commandName, metaData);
        }

        #endregion Protected Methods
    }
}