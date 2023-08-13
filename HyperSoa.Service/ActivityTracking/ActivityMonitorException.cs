﻿using System.Runtime.Serialization;

namespace HyperSoa.Service.ActivityTracking
{
    [Serializable]
    internal class ActivityMonitorException : Exception
    {
        public ActivityMonitorException() { }
        public ActivityMonitorException(string message) : base(message) { }
        public ActivityMonitorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public ActivityMonitorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
