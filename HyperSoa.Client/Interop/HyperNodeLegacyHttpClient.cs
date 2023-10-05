using System.Net.Http.Headers;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Legacy;
using HyperSoa.Contracts.Legacy.Extensions;
using System.Text;

namespace HyperSoa.Client.Interop
{
    public class HyperNodeLegacyHttpClient : IHyperNodeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public HyperNodeLegacyHttpClient(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<HyperNodeMessageResponse> ProcessMessageAsync(HyperNodeMessageRequest message)
        {
            try
            {
                var hrmRequest = new HttpRequestMessage {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_endpoint),
                    Content = new StringContent(
                        message.ToLegacyHyperNodeMessageRequest().SerializeToSoapXml()
                    )
                };
                hrmRequest.Headers.Add("SOAPAction", "http://tempuri.org/IHyperNodeService/ProcessMessage");
                hrmRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml") { CharSet = Encoding.UTF8.WebName };
                                
                var httpResponse = await _httpClient.SendAsync(hrmRequest).ConfigureAwait(false);
                return new LegacyHyperNodeMessageResponse(
                    await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false)
                ).ToHyperNodeMessageResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending response to server.", ex);
            }
        }
    }
}
