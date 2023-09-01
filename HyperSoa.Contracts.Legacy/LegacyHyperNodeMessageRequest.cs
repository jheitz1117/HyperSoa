using System.Xml.Linq;

namespace HyperSoa.Contracts.Legacy
{
    public class LegacyHyperNodeMessageRequest
    {
        public string? CreatedByAgentName { get; set; }
        public string? CommandName { get; set; }
        public string? CommandRequestString { get; set; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }



        private static XNamespace xn_s = "http://schemas.xmlsoap.org/soap/envelope/";
        private static XNamespace xn_t = "http://tempuri.org/";
        private static XNamespace xn_a = "http://schemas.datacontract.org/2004/07/Hyper.NodeServices.Contracts";
        private static XNamespace xn_i = "http://www.w3.org/2001/XMLSchema-instance";

        public LegacyHyperNodeMessageRequest() { }
        public LegacyHyperNodeMessageRequest(Stream reqStream) {
            XElement root = XElement.Load(reqStream);
            XElement? msg = root.Descendants(xn_s + "Body").Descendants(xn_t + "ProcessMessage").Descendants(xn_t + "message").FirstOrDefault();
            if (msg != null) {
                CreatedByAgentName = msg.Element(xn_a + "CreatedByAgentName")?.Value ?? "";
                CommandName = msg.Element(xn_a + "CommandName")?.Value ?? "";
                CommandRequestString = msg.Element(xn_a + "CommandRequestString")?.Value ?? "";
                ProcessOptionFlags = Enum.Parse<MessageProcessOptionFlags>(msg.Element(xn_a + "ProcessOptionFlags")?.Value ?? "None");
            } else {
                throw new HttpRequestException("Soap message not found in request");
            }
        }

        public string SerializeToSoapXml() {
            XElement env =
                new XElement(xn_s + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "s", xn_s.NamespaceName),
                    new XElement(xn_s + "Body",
                        new XElement(xn_t + "ProcessMessage",
                            new XAttribute("xmlns", xn_t.NamespaceName),
                            new XElement(xn_t + "message",
                                new XAttribute(XNamespace.Xmlns + "a", xn_a.NamespaceName),
                                new XAttribute(XNamespace.Xmlns + "i", xn_i.NamespaceName),
                                new XElement(xn_a + "CommandName", CommandName),
                                new XElement(xn_a + "CommandRequestString", CommandRequestString),
                                new XElement(xn_a + "CreatedByAgentName", CreatedByAgentName),
                                new XElement(xn_a + "ProcessOptionFlags", ProcessOptionFlags)
                            )
                        )
                    )
                );

            return env.ToString(SaveOptions.DisableFormatting);
        }
    }
}
