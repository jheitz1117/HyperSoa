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

        public HostingTestClient(string clientApplicationName, HttpClient httpClient, string endpoint)
            : base(new HyperNodeHttpClient(httpClient, endpoint))
        {
            ClientApplicationName = clientApplicationName;
        }

        #region Public Methods

        public async Task<string> LongRunningCommandRemoteAsync(LongRunningCommandRequest request)
        {
            return await LongRunningCommandRemoteAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> LongRunningCommandRemoteAsync(ICommandMetaData metaData)
        {
            return await ExecuteCommandRemoteAsync<LongRunningCommandRequest>(
                "LongRunningCommand",
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<string> LongRunningSingletonCommandRemoteAsync(LongRunningCommandRequest request)
        {
            return await LongRunningSingletonCommandRemoteAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> LongRunningSingletonCommandRemoteAsync(ICommandMetaData metaData)
        {
            return await ExecuteCommandRemoteAsync<LongRunningCommandRequest>(
                "LongRunningSingletonCommand",
                metaData
            ).ConfigureAwait(false);
        }

        public async Task<ComplexCommandResponse> ComplexCommandAsync(ComplexCommandRequest request)
        {
            return await ComplexCommandAsync(
                request.CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
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

        public async Task<string> NoContractCommandRemoteAsync()
        {
            return await NoContractCommandRemoteAsync(
                CommandMetaData.Default().CreatedBy(ClientApplicationName)
            ).ConfigureAwait(false);
        }

        public async Task<string> NoContractCommandRemoteAsync(ICommandMetaData metaData)
        {
            return await ExecuteCommandRemoteAsync(
                "NoContractCommand",
                metaData
            ).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected async Task<string> ExecuteCommandRemoteAsync<T>(string commandName, ICommandMetaData? metaData = null)
            where T : ICommandRequest
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<T, ICommandResponse>();

            return await base.ExecuteCommandRemoteAsync(
                commandName,
                metaData
            ).ConfigureAwait(false);
        }

        protected async Task ExecuteCommandAsync<T>(string commandName, ICommandMetaData? metaData = null)
            where T : ICommandRequest
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<T, ICommandResponse>();

            await base.ExecuteCommandAsync(
                commandName,
                metaData
            ).ConfigureAwait(false);
        }

        public override async Task<TResponse> GetCommandResponseAsync<TRequest, TResponse>(string commandName, ICommandMetaData? metaData = null)
        {
            // By default, use protobuf for serialization
            if (metaData is { Serializer: null })
                metaData.Serializer = new ProtoContractSerializer<TRequest, TResponse>();

            return await base.GetCommandResponseAsync<TRequest, TResponse>(
                commandName,
                metaData
            ).ConfigureAwait(false);
        }

        #endregion Protected Methods
    }
}