﻿using System.Runtime.Serialization;

namespace HyperSoa.Service
{
    [Serializable]
    internal class ActivityTrackerInitializationException : Exception
    {
        public ActivityTrackerInitializationException() { }
        public ActivityTrackerInitializationException(string message) : base(message) { }
        public ActivityTrackerInitializationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public ActivityTrackerInitializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
