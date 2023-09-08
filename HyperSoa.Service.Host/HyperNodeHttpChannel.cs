using System.Net;
using HyperSoa.Contracts;
using HyperSoa.Service.Host.Configuration;
using Microsoft.Extensions.Logging;

namespace HyperSoa.Service.Host
{
    internal class HyperNodeHttpChannel : IHyperNodeChannel
    {
        private readonly HyperNodeHttpEndpoint[] _httpEndpoints;
        private readonly ILogger<HyperNodeHttpChannel> _logger;
        private readonly HttpListener _listener;

        protected IHyperNodeService ServiceInstance { get; }
        public IEnumerable<string> Endpoints => _listener.Prefixes;

        public HyperNodeHttpChannel(HyperNodeHttpEndpoint[] httpEndpoints, IHyperNodeService serviceInstance, ILogger<HyperNodeHttpChannel> logger)
        {
            _httpEndpoints = httpEndpoints ?? throw new ArgumentNullException(nameof(httpEndpoints));
            ServiceInstance = serviceInstance ?? throw new ArgumentNullException(nameof(serviceInstance));
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

        #region Protected Methods

        protected virtual async Task ProcessRequestAsync(HttpListenerContext httpContext)
        {
            byte[] responseBytes;

            try
            {
                var request = ProtoBuf.Serializer.Deserialize<HyperNodeMessageRequest>(
                    httpContext.Request.InputStream
                ) ?? throw new Exception("Invalid request message");

                var hyperResp = await ServiceInstance.ProcessMessageAsync(
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
                _logger.LogError(ex, "An exception was thrown while attempting to process a HyperNode message over HTTP.");

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

        #endregion Protected Methods
    }
}
