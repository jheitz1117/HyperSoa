using System.Runtime.Serialization;

namespace HyperSoa.Service.Configuration
{
    [Serializable]
    internal class DuplicateActivityMonitorException : Exception
    {
        public DuplicateActivityMonitorException() { }
        public DuplicateActivityMonitorException(string message) : base(message) { }
        public DuplicateActivityMonitorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public DuplicateActivityMonitorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
