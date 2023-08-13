using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.EnableTaskProgressCache"/> system command.
    /// </summary>
    [ProtoContract]
    public class EnableTaskProgressCacheRequest : ICommandRequest
    {
        /// <summary>
        /// Indicates whether to enable or disable the task progress cache.
        /// </summary>
        [ProtoMember(1)]
        public bool Enable { get; set; }
    }
}
