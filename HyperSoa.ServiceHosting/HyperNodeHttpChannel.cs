using System.Net;
using HyperSoa.Contracts;
using HyperSoa.ServiceHosting.Configuration;
using Microsoft.Extensions.Logging;

namespace HyperSoa.ServiceHosting
{
    internal class HyperNodeHttpChannel : IHyperNodeChannel
    {
        private readonly IHyperNodeHttpEndpoint[] _httpEndpoints;
        private readonly IHyperNodeService _serviceInstance;
        private readonly ILogger<HyperNodeHttpChannel>? _logger;
        private readonly HttpListener _listener;

        public IEnumerable<string> Endpoints => _listener.Prefixes;

        public HyperNodeHttpChannel(IHyperNodeHttpEndpoint[] httpBindings, IHyperNodeService serviceInstance, ILogger<HyperNodeHttpChannel>? logger)
        {
            _httpEndpoints = httpBindings ?? throw new ArgumentNullException(nameof(httpBindings));
            _serviceInstance = serviceInstance ?? throw new ArgumentNullException(nameof(serviceInstance));
            _logger = logger;

            _listener = new HttpListener();
        }

        #region Public Methods

        public void Open()
        {
            foreach (var httpBinding in _httpEndpoints)
            {
                _listener.Prefixes.Add(
                    $"{httpBinding.Uri}{(httpBinding.Uri?.EndsWith("/") == true ? "" : "/")}"
                ); 
            }
            
            _listener.Start();
            
            Task.Run(ListenForRequests);
        }

        public async Task ListenForRequests()
        {
            while (_listener.IsListening)
            {
                var httpContext = await _listener.GetContextAsync().ConfigureAwait(false);
                _ = Task.Run(() => ProcessRequestAsync(httpContext));
            }
        }

        public void Abort()
        {
            _listener.Abort();
        }

        public void Close()
        {
            _listener.Close();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task ProcessRequestAsync(HttpListenerContext httpContext)
        {
            byte[] responseBytes;

            try
            {
                var request = ProtoBuf.Serializer.Deserialize<HyperNodeMessageRequest>(
                    httpContext.Request.InputStream
                ) ?? throw new Exception("Invalid request message");

                var hyperResp = await _serviceInstance.ProcessMessageAsync(
                    request
                ).ConfigureAwait(false);

                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, hyperResp);
                    responseBytes = ms.ToArray();
                }

                httpContext.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An exception was thrown while attempting to process a HyperNode message over HTTP.");

                var errorResponse = new HyperNodeMessageResponse
                {
                    RespondingNodeName = nameof(HyperNodeHttpChannel),
                    NodeAction = HyperNodeActionType.Rejected,
                    NodeActionReason = HyperNodeActionReasonType.CommunicationException
                };

                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, errorResponse);
                    responseBytes = ms.ToArray();
                }

                httpContext.Response.StatusCode = 500;
            }

            httpContext.Response.ContentLength64 = responseBytes.Length;
            
            await httpContext.Response.OutputStream.WriteAsync(
                responseBytes
            ).ConfigureAwait(false);
            
            await httpContext.Response.OutputStream.FlushAsync().ConfigureAwait(false);

            httpContext.Response.Close();
        }

        #endregion Private Methods
    }
}
