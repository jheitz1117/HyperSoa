using System.Runtime.Serialization;

namespace HyperSoa.ServiceHosting
{
    [Serializable]
    public class CommunicationException : SystemException
    {
        //
        // Summary:
        //     Initializes a new instance of the System.ServiceModel.CommunicationException
        //     class.
        public CommunicationException()
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.ServiceModel.CommunicationException
        //     class, using the specified message.
        //
        // Parameters:
        //   message:
        //     The description of the error condition.
        public CommunicationException(string message)
            : base(message)
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.ServiceModel.CommunicationException
        //     class, using the specified message and the inner exception.
        //
        // Parameters:
        //   message:
        //     The description of the error condition.
        //
        //   innerException:
        //     The inner exception to be used.
        public CommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.ServiceModel.CommunicationException
        //     class, using the specified serialization information and context objects.
        //
        // Parameters:
        //   info:
        //     Information relevant to the deserialization process.
        //
        //   context:
        //     The context of the deserialization process.
        protected CommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
