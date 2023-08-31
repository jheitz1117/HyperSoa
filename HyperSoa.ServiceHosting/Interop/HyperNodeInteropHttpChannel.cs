using System.Net;
using System.Text;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Legacy;
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
                    var legacyRequest = DeserializeSoapXml(httpContext.Request.InputStream);

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

        private LegacyHyperNodeMessageRequest DeserializeSoapXml(Stream stream)
        {
            return new LegacyHyperNodeMessageRequest(stream);
        }

        private byte[] SerializeToSoapXml(LegacyHyperNodeMessageResponse legacyResponse)
        {
            return Encoding.UTF8.GetBytes(legacyResponse.SerializeToSoapXml());
        }
        
        #endregion Private Methods
    }
}
