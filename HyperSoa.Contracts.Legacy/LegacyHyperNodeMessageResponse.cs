using System.Xml.Linq;
using System.Xml;

namespace HyperSoa.Contracts.Legacy {
    public class LegacyHyperNodeMessageResponse {
        public string? TaskId { get; set; }
        public string? RespondingNodeName { get; set; }
        public TimeSpan? TotalRunTime { get; set; }
        public HyperNodeActionType NodeAction { get; set; }
        public HyperNodeActionReasonType NodeActionReason { get; set; }
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
        public List<HyperNodeActivityItem>? TaskTrace { get; set; }
        public string? CommandResponseString { get; set; }



        private static XNamespace xn_s = "http://schemas.xmlsoap.org/soap/envelope/";
        private static XNamespace xn_t = "http://tempuri.org/";
        private static XNamespace xn_a = "http://schemas.datacontract.org/2004/07/Hyper.NodeServices.Contracts";
        private static XNamespace xn_i = "http://www.w3.org/2001/XMLSchema-instance";

        public LegacyHyperNodeMessageResponse() { }

        public LegacyHyperNodeMessageResponse(Stream respStream) {
            XElement root = XElement.Load(respStream);
            XElement? msg = root.Descendants(xn_s + "Body").Descendants(xn_t + "ProcessMessageResponse").Descendants(xn_t + "ProcessMessageResult").FirstOrDefault();
            if (msg != null) {
                CommandResponseString = msg.Element(xn_a + "CommandResponseString")?.Value ?? "";
                NodeAction = Enum.Parse<HyperNodeActionType>(msg.Element(xn_a + "NodeAction")?.Value ?? "None");
                NodeActionReason = Enum.Parse<HyperNodeActionReasonType>(msg.Element(xn_a + "NodeActionReason")?.Value ?? "None");
                ProcessStatusFlags = Enum.Parse<MessageProcessStatusFlags>(msg.Element(xn_a + "ProcessStatusFlags")?.Value ?? "None");
                RespondingNodeName = msg.Element(xn_a + "RespondingNodeName")?.Value ?? "";
                TaskId = msg.Element(xn_a + "TaskId")?.Value ?? "";
                TaskTrace = msg.Element(xn_a + "TaskTrace").Elements(xn_a + "HyperNodeActivityItem").Select(tt =>
                    new HyperNodeActivityItem {
                        Agent = tt.Element(xn_a + "Agent")?.Value ?? "",
                        Elapsed = string.IsNullOrEmpty(tt.Element(xn_a + "Elapsed")?.Value) ? null : XmlConvert.ToTimeSpan(tt.Element(xn_a + "Elapsed")?.Value),
                        EventDateTime = string.IsNullOrEmpty(tt.Element(xn_a + "EventDateTime")?.Value) ? DateTime.MinValue : DateTime.Parse(tt.Element(xn_a + "EventDateTime")?.Value),
                        EventDescription = tt.Element(xn_a + "EventDescription")?.Value ?? "",
                        EventDetail = tt.Element(xn_a + "EventDetail")?.Value ?? "",
                        ProgressPart = string.IsNullOrEmpty(tt.Element(xn_a + "ProgressPart")?.Value) ? null : double.Parse(tt.Element(xn_a + "ProgressPart")?.Value),
                        ProgressTotal = string.IsNullOrEmpty(tt.Element(xn_a + "ProgressTotal")?.Value) ? null : double.Parse(tt.Element(xn_a + "ProgressTotal")?.Value)
                    }
                ).ToList();
                TotalRunTime = string.IsNullOrEmpty(msg.Element(xn_a + "TotalRunTime")?.Value) ? null : XmlConvert.ToTimeSpan(msg.Element(xn_a + "TotalRunTime")?.Value);
            } else {
                throw new HttpRequestException("Soap message not found in response");
            }
        }

        public string SerializeToSoapXml() {
            XElement env =
                new XElement(xn_s + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "s", xn_s.NamespaceName),
                    new XElement(xn_s + "Body",
                        new XElement(xn_t + "ProcessMessageResponse",
                            new XAttribute("xmlns", xn_t.NamespaceName),
                            new XElement(xn_t + "ProcessMessageResult",
                                new XAttribute(XNamespace.Xmlns + "a", xn_a.NamespaceName),
                                new XAttribute(XNamespace.Xmlns + "i", xn_i.NamespaceName),
                                new XElement(xn_a + "CommandResponseString", CommandResponseString),
                                new XElement(xn_a + "NodeAction", NodeAction),
                                new XElement(xn_a + "NodeActionReason", NodeActionReason),
                                new XElement(xn_a + "ProcessStatusFlags", ProcessStatusFlags),
                                new XElement(xn_a + "RespondingNodeName", RespondingNodeName),
                                new XElement(xn_a + "TaskId", TaskId),
                                new XElement(xn_a + "TaskTrace", TaskTrace?.Select(tt =>
                                        new XElement(xn_a + "HyperNodeActivityItem",
                                            new XElement(xn_a + "Agent", tt.Agent),
                                            new XElement(xn_a + "Elapsed", (tt.Elapsed == null ? "" : XmlConvert.ToString((TimeSpan)tt.Elapsed)),
                                            new XElement(xn_a + "EventDateTime", tt.EventDateTime),
                                            new XElement(xn_a + "EventDescription", tt.EventDescription),
                                            new XElement(xn_a + "EventDetail", tt.EventDetail),
                                            new XElement(xn_a + "ProgressPart", tt.ProgressPart),
                                            new XElement(xn_a + "ProgressTotal", tt.ProgressTotal)
                                        )
                                ))),
                                new XElement(xn_a + "TotalRunTime", (TotalRunTime == null ? "" : XmlConvert.ToString((TimeSpan)TotalRunTime)))
                            )
                        )
                    )
                );
            return env.ToString(SaveOptions.DisableFormatting);
        }
    }
}
