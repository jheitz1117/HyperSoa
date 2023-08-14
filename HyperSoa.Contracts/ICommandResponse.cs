namespace HyperSoa.Contracts
{
    /// <summary>
    /// Defines a class that is returned from a custom command module.
    /// </summary>
    public interface ICommandResponse
    {
        /// <summary>
        /// A bitwise combination of <see cref="MessageProcessStatusFlags"/> values indicating what happened while the command was running.
        /// </summary>
        MessageProcessStatusFlags ProcessStatusFlags { get; set; }
    }
}
