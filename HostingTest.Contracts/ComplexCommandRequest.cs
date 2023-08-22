using System.Runtime.Serialization;
using HyperSoa.Contracts;
using ProtoBuf;

namespace HostingTest.Contracts
{
    [ProtoContract]
    [DataContract]
    public class ComplexCommandRequest : ICommandRequest
    {
        [ProtoMember(1)]
        [DataMember]
        public string MyString { get; set; }
        
        [ProtoMember(2)]
        [DataMember]
        public DateTime MyDateTime { get; set; }

        // No attribute for testing purposes (to confirm it's not serialized)
        public int MyInt32 { get; set; }
        
        [ProtoMember(3)]
        [DataMember]
        public TimeSpan MyTimeSpan { get; set; }
    }
}