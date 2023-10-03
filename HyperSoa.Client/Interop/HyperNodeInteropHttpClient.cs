using HyperSoa.Client.Configuration;
using HyperSoa.Contracts;
using HyperSoa.Contracts.Legacy;
using HyperSoa.Contracts.Legacy.Extensions;
//using Microsoft.Extensions.Options;
using System.Text;

namespace HyperSoa.Client
{
    public class HyperNodeInteropHttpClient
    {
        public static async Task<HyperNodeMessageResponse> ProcessMessageAsync(HttpClient httpClient, string endpoint, HyperNodeMessageRequest message)
        {
            try
            {
                HttpRequestMessage hrmRequest = new HttpRequestMessage {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(endpoint),
                    Content = new StringContent(message.ToLegacyHyperNodeMessageRequest().SerializeToSoapXml()) //, _hyperServerSettings.MessageFormat)
                };
                hrmRequest.Headers.Add("SOAPAction", "http://tempuri.org/IHyperNodeService/ProcessMessage");
                hrmRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml") { CharSet = Encoding.UTF8.WebName };
                                
                var httpResponse = await httpClient.SendAsync(hrmRequest).ConfigureAwait(false);
                return (new LegacyHyperNodeMessageResponse(await httpResponse.Content.ReadAsStreamAsync())).ToHyperNodeMessageResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending response to server.", ex);
            }
        }
    }
}
