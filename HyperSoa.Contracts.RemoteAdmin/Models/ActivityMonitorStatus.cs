using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin.Models
{
    /// <summary>
    /// Describes the status of a custom activity monitor.
    /// </summary>
    [ProtoContract]
    public class ActivityMonitorStatus
    {
        /// <summary>
        /// The name of the custom activity monitor.
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }
        
        /// <summary>
        /// Indicates whether the custom activity monitor is enabled or disabled.
        /// </summary>
        [ProtoMember(2)]
        public bool Enabled { get; set; }
    }
}
