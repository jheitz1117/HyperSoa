using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public class HyperNodeClient : IHyperNodeService
    {
        private static readonly HttpClient HttpClient = new();

        private readonly string _endpoint;

        public HyperNodeClient(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task<HyperNodeMessageResponse> ProcessMessageAsync(HyperNodeMessageRequest message)
        {
            try
            {
                ByteArrayContent? msgContent;
                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, message);
                    msgContent = new ByteArrayContent(ms.ToArray());
                }
                
                var httpResponse = await HttpClient.PostAsync(
                    _endpoint,
                    msgContent
                ).ConfigureAwait(false);
                
                return ProtoBuf.Serializer.Deserialize<HyperNodeMessageResponse>(
                    await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false)
                ) ?? throw new InvalidOperationException("Invalid response message");
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending response to server.", ex);
            }
        }
    }
}