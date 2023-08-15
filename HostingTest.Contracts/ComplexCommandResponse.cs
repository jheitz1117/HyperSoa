using HyperSoa.Contracts;
using ProtoBuf;

namespace HostingTest.Contracts
{
    [ProtoContract]
    public class ComplexCommandResponse : ICommandResponse
    {
        [ProtoMember(1)]
        public MessageProcessStatusFlags ProcessStatusFlags { get; set; }
        
        [ProtoMember(2)]
        public DateTime EightDaysLaterThanRequest { get; set; }
        
        [ProtoMember(3)]
        public TimeSpan FortyYearTimeSpan { get; set; }
        
        [ProtoMember(4)]
        public int FiveHundredLessThanRequest { get; set; }
        
        [ProtoMember(5)]
        public string ResponseStringNotTheSame { get; set; }
    }
}
