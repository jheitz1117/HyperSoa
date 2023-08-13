namespace HyperSoa.ServiceHosting
{
    /// <summary>
    /// Defines a method to handle exceptions thrown by a <see cref="HyperCoreServiceHost"/> instance.
    /// </summary>
    public interface IServiceHostExceptionHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Exception"/>.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to handle.</param>
        void HandleException(Exception ex);
    }
}
