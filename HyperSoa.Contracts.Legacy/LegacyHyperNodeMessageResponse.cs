using System.Xml.Linq;
using System.Xml;

namespace HyperSoa.Contracts.Legacy {
    public class LegacyHyperNodeMessageResponse : LegacyHyperNodeMessage {
        public string? TaskId { get; set; }
        public string? RespondingNodeName { get; set; }
        public TimeSpan? TotalRunTime { get; set; }
        public HyperNodeActionType NodeAction { get; set; }
        public HyperNodeActionReasonType NodeActionReason { get; set; }
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
        public List<HyperNodeActivityItem>? TaskTrace { get; set; }
        public string? CommandResponseString { get; set; }


        public LegacyHyperNodeMessageResponse() { }
        public LegacyHyperNodeMessageResponse(Stream respStream) {
            XElement root = XElement.Load(respStream);
            XElement? msg = root.Descendants(xn_s + "Body").Descendants(xn_t + "ProcessMessageResponse").Descendants(xn_t + "ProcessMessageResult").FirstOrDefault();
            if (msg != null) {
                CommandResponseString = ParseElementValue<string?>(msg.Element(xn_a + nameof(CommandResponseString)));
                NodeAction = ParseElementValue<HyperNodeActionType>(msg.Element(xn_a + nameof(NodeAction)));
                NodeActionReason = ParseElementValue<HyperNodeActionReasonType>(msg.Element(xn_a + nameof(NodeActionReason)));
                ProcessStatusFlags = ParseElementValue<MessageProcessStatusFlags>(msg.Element(xn_a + nameof(ProcessStatusFlags)));
                RespondingNodeName = ParseElementValue<string?>(msg.Element(xn_a + nameof(RespondingNodeName)));
                TaskId = ParseElementValue<string?>(msg.Element(xn_a + nameof(TaskId)));
                TaskTrace = msg.Element(xn_a + nameof(TaskTrace))?.Elements(xn_a + nameof(HyperNodeActivityItem)).Select(tt =>
                    new HyperNodeActivityItem {
                        Agent = ParseElementValue<string?>(tt.Element(xn_a + nameof(HyperNodeActivityItem.Agent))),
                        Elapsed = ParseElementValue<TimeSpan>(tt.Element(xn_a + nameof(HyperNodeActivityItem.Elapsed))),
                        EventDateTime = ParseElementValue<DateTime>(tt.Element(xn_a + nameof(HyperNodeActivityItem.EventDateTime))),
                        EventDescription = ParseElementValue<string?>(tt.Element(xn_a + nameof(HyperNodeActivityItem.EventDescription))),
                        EventDetail = ParseElementValue<string?>(tt.Element(xn_a + nameof(HyperNodeActivityItem.EventDetail))),
                        ProgressPart = ParseElementValue<double?>(tt.Element(xn_a + nameof(HyperNodeActivityItem.ProgressPart))),
                        ProgressTotal = ParseElementValue<double?>(tt.Element(xn_a + nameof(HyperNodeActivityItem.ProgressTotal)))
                    }
                ).ToList();
                TotalRunTime = ParseElementValue<TimeSpan>(msg.Element(xn_a + nameof(TotalRunTime)));
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
                                new XElement(xn_a + nameof(CommandResponseString), CommandResponseString ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(NodeAction), NodeAction),
                                new XElement(xn_a + nameof(NodeActionReason), NodeActionReason),
                                new XElement(xn_a + nameof(ProcessStatusFlags), ProcessStatusFlags),
                                new XElement(xn_a + nameof(RespondingNodeName), RespondingNodeName ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(TaskId), TaskId ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(TaskTrace), TaskTrace?.Select(tt =>
                                    new XElement(xn_a + nameof(HyperNodeActivityItem),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.Agent), tt.Agent ?? (object?)new XAttribute(xn_i + nil, true)),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.Elapsed), (tt.Elapsed == null ? new XAttribute(xn_i + nil, true) : XmlConvert.ToString((TimeSpan)tt.Elapsed))),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.EventDateTime), tt.EventDateTime),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.EventDescription), tt.EventDescription ?? (object?)new XAttribute(xn_i + nil, true)),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.EventDetail), tt.EventDetail ?? (object?)new XAttribute(xn_i + nil, true)),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.ProgressPart), tt.ProgressPart ?? (object?)new XAttribute(xn_i + nil, true)),
                                        new XElement(xn_a + nameof(HyperNodeActivityItem.ProgressTotal), tt.ProgressTotal ?? (object?)new XAttribute(xn_i + nil, true))
                                    )
                                ) ?? (object?)new XAttribute(xn_i + nil, true)),
                                new XElement(xn_a + nameof(TotalRunTime), (TotalRunTime == null ? new XAttribute(xn_i + nil, true) : XmlConvert.ToString((TimeSpan)TotalRunTime)))
                            )
                        )
                    )
                );
            return env.ToString(SaveOptions.DisableFormatting);
        }
    }
}
