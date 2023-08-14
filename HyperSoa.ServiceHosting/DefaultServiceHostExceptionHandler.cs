using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HyperSoa.ServiceHosting
{
    /// <summary>
    /// Default implementation of <see cref="IServiceHostExceptionHandler"/> that writes exceptions to the <see cref="Trace"/>.
    /// </summary>
    public sealed class DefaultServiceHostExceptionHandler : IServiceHostExceptionHandler
    {
        private readonly ILogger<DefaultServiceHostExceptionHandler> _logger;

        public DefaultServiceHostExceptionHandler(ILogger<DefaultServiceHostExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calls <see cref="Exception.ToString()"/> on the specified <see cref="Exception"/> and writes the results to <see cref="Trace"/>.<see cref="Trace.WriteLine(object)"/>.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to trace.</param>
        public void HandleException(Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown by the service host.");
        }
    }
}
