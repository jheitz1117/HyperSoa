using ProtoBuf;

namespace HyperSoa.Contracts.RemoteAdmin
{
    /// <summary>
    /// <see cref="ICommandRequest"/> for the <see cref="RemoteAdminCommandName.SetTaskProgressCacheDuration"/> system command.
    /// </summary>
    [ProtoContract]
    public class SetTaskProgressCacheDurationRequest : ICommandRequest
    {
        /// <summary>
        /// The new sliding expiration to set for items in the task progress cache.
        /// </summary>
        [ProtoMember(1)]
        public TimeSpan CacheDuration { get; set; }
    }
}
