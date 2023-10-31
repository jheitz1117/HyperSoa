using System.Xml.Linq;

namespace HyperSoa.Contracts.Legacy
{
    public class LegacyHyperNodeMessageRequest : LegacyHyperNodeMessage {
        public string? CreatedByAgentName { get; set; }
        public string? CommandName { get; set; }
        public string? CommandRequestString { get; set; }
        public MessageProcessOptionFlags ProcessOptionFlags { get; set; }


        public LegacyHyperNodeMessageRequest() { }
        public LegacyHyperNodeMessageRequest(Stream reqStream) {
            XElement root = XElement.Load(reqStream);
            XElement? msg = root.Descendants(xn_s + "Body").Descendants(xn_t + "ProcessMessage").Descendants(xn_t + "message").FirstOrDefault();
            if (msg != null) {
                CreatedByAgentName = ParseElementValue<string?>(msg.Element(xn_a + nameof(CreatedByAgentName)));
                CommandName = ParseElementValue<string?>(msg.Element(xn_a + nameof(CommandName)));
                CommandRequestString = ParseElementValue<string?>(msg.Element(xn_a + nameof(CommandRequestString)));
                ProcessOptionFlags = ParseElementValue<MessageProcessOptionFlags>(msg.Element(xn_a + nameof(ProcessOptionFlags)));
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
                                new XElement(xn_a + nameof(CommandName), CommandName ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(CommandRequestString), CommandRequestString ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(CreatedByAgentName), CreatedByAgentName ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(ProcessOptionFlags), ProcessOptionFlags.ToString().Replace(",", ""))
                            )
                        )
                    )
                );

            return env.ToString(SaveOptions.DisableFormatting);
        }
    }
}
