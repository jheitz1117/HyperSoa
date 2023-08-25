using System.Net;
using HyperSoa.Contracts;
using HyperSoa.ServiceHosting.Configuration;
using HyperSoa.ServiceHosting.Extensions;
using Microsoft.Extensions.Logging;

namespace HyperSoa.ServiceHosting.Interop
{
    internal sealed class HyperNodeInteropHttpChannel : HyperNodeHttpChannel
    {
        private readonly ILogger<HyperNodeInteropHttpChannel> _logger;

        public HyperNodeInteropHttpChannel(HyperNodeHttpEndpoint[] httpEndpoints, IHyperNodeService serviceInstance, ILogger<HyperNodeInteropHttpChannel> logger)
            : base(httpEndpoints, serviceInstance, logger)
        {
            _logger = logger;
        }

        #region Protected Methods
        
        protected override async Task ProcessRequestAsync(HttpListenerContext httpContext)
        {
            if (httpContext.Request.Headers.Get("SOAPAction") != null)
            {
                byte[] responseBytes;

                try
                {
                    var legacyRequest = DeserializeSoapXml();

                    var legacyResponse = (
                        await ServiceInstance.ProcessMessageAsync(
                            legacyRequest.ToHyperNodeMessageRequest()
                        ).ConfigureAwait(false)
                    ).ToLegacyHyperNodeMessageResponse();

                    responseBytes = SerializeToSoapXml(legacyResponse);

                    httpContext.Response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception was thrown while attempting to process a HyperNode message over HTTP.");

                    var errorResponse = new LegacyHyperNodeMessageResponse
                    {
                        RespondingNodeName = nameof(HyperNodeInteropHttpChannel),
                        NodeAction = HyperNodeActionType.Rejected,
                        NodeActionReason = HyperNodeActionReasonType.CommunicationException
                    };

                    responseBytes = SerializeToSoapXml(errorResponse);
                    
                    httpContext.Response.StatusCode = 500;
                }

                httpContext.Response.ContentLength64 = responseBytes.Length;
            
                await httpContext.Response.OutputStream.WriteAsync(
                    responseBytes
                ).ConfigureAwait(false);
            
                await httpContext.Response.OutputStream.FlushAsync().ConfigureAwait(false);

                httpContext.Response.Close();
            }
            else
            {
                await base.ProcessRequestAsync(httpContext).ConfigureAwait(false);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private LegacyHyperNodeMessageRequest DeserializeSoapXml()
        {
            // TODO: Deserialize SOAP XML as LegacyHyperNodeMessageRequest
            return new LegacyHyperNodeMessageRequest();
        }

        private byte[] SerializeToSoapXml(LegacyHyperNodeMessageResponse legacyResponse)
        {
            // TODO: Serialize LegacyHyperNodeMessageResponse as SOAP XML
            return Array.Empty<byte>();
        }
        
        #endregion Private Methods
    }
}
