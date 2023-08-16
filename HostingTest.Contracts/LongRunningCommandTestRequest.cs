using HyperSoa.Contracts;
using ProtoBuf;

namespace HostingTest.Contracts
{
    /// <summary>
    /// Specifies parameters for a long-running command module.
    /// </summary>
    [ProtoContract]
    public class LongRunningCommandRequest : ICommandRequest
    {
        /// <summary>
        /// Specifies the maximum run time for the command. If this value is null, a default value is chosen by the command module.
        /// </summary>
        [ProtoMember(1)]
        public TimeSpan? TotalRunTime { get; set; }

        /// <summary>
        /// Specifies the minimum amount of time to wait between progress reports. If this value is null, a default minimum is chosen by the command module.
        /// </summary>
        [ProtoMember(2)]
        public TimeSpan? MinimumSleepInterval { get; set; }

        /// <summary>
        /// Specifies the maximum amount of time to wait between progress reports. If this value is null, a default maximum is chosen by the command module.
        /// </summary>
        [ProtoMember(3)]
        public TimeSpan? MaximumSleepInterval { get; set; }
    }
}
