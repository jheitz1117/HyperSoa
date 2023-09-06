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
            return await RunCommandAsync<LongRunningCommandRequest>(
                "LongRunningCommand",
                metaData
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
            return await RunCommandAsync<LongRunningCommandRequest>(
                "LongRunningSingletonCommand",
                metaData
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

        public async Task NoContractCommandAsync()
        {
            await NoContractCommandAsync(
                CommandMetaData.Default().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task NoContractCommandAsync(ICommandMetaData metaData)
        {
            await ExecuteCommandAsync<EmptyCommandRequest>(
                "NoContractCommand",
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<string> RunNoContractCommandAsync()
        {
            return await RunNoContractCommandAsync(
                CommandMetaData.Default().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> RunNoContractCommandAsync(ICommandMetaData metaData)
        {
            return await RunCommandAsync(
                "NoContractCommand",
                metaData
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected Task<string> RunCommandAsync<T>(string commandName, ICommandMetaData? metaData = null)
            where T : ICommandRequest
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<T, ICommandResponse>();

            return base.RunCommandAsync(commandName, metaData);
        }

        protected Task ExecuteCommandAsync<T>(string commandName, ICommandMetaData? metaData = null)
            where T : ICommandRequest
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<T, ICommandResponse>();

            return base.ExecuteCommandAsync(commandName, metaData);
        }

        public override Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();

            return base.GetCommandResponseAsync<TRequest, TResponse>(commandName, metaData);
        }

        #endregion Protected Methods
    }
}