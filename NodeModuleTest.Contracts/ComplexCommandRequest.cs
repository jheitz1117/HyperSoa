using HyperSoa.Contracts;
using ProtoBuf;

namespace NodeModuleTest.Contracts
{
    [ProtoContract]
    public class ComplexCommandRequest : ICommandRequest
    {
        [ProtoMember(1)]
        public string MyString { get; set; }
        
        [ProtoMember(2)]
        public DateTime MyDateTime { get; set; }

        public int MyInt32 { get; set; }
        
        [ProtoMember(3)]
        public TimeSpan MyTimeSpan { get; set; }
    }
}