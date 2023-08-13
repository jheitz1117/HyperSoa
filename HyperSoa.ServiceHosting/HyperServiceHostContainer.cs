using static HyperSoa.ServiceHosting.HyperCoreServiceHost;

namespace HyperSoa.ServiceHosting
{
    /// <summary>
    /// Simplifies self-hosting for <see cref="HyperCoreServiceHost"/> objects.
    /// Supports singleton service contract implementations that implement <see cref="IDisposable"/>.
    /// </summary>
    public sealed class HyperServiceHostContainer
    {
        private HyperCoreServiceHost? _host;

        private readonly IServiceHostFactory _hostFactory;
        private readonly IServiceHostExceptionHandler _timeoutExceptionHandler;
        private readonly IServiceHostExceptionHandler _communicationExceptionHandler;
        private readonly IServiceHostExceptionHandler _genericExceptionHandler;

        /// <summary>
        /// List of endpoints on which the <see cref="HyperCoreServiceHost"/> is listening.
        /// </summary>
        public IEnumerable<string> Endpoints => _host?.Endpoints ?? Array.Empty<string>();

        /// <summary>
        /// Indicates whether the <see cref="Stop()"/> method will try to dispose the service hosted by
        /// the internal <see cref="HyperCoreServiceHost"/>.
        /// </summary>
        public bool DisposeServiceOnStop { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="HyperServiceHostContainer"/> with the specified factory method and <see cref="IServiceHostExceptionHandler"/> implementation.
        /// </summary>
        /// <param name="factory">The delegate that is invoked to create the <see cref="HyperCoreServiceHost"/> object to wrap.</param>
        /// <param name="exceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when any is thrown.</param>
        public HyperServiceHostContainer(Func<HyperCoreServiceHost> factory, IServiceHostExceptionHandler exceptionHandler)
            : this(factory, exceptionHandler, exceptionHandler, exceptionHandler)
        { }

        /// <summary>
        /// Initializes an instance of <see cref="HyperServiceHostContainer"/> with the specified factory method and <see cref="IServiceHostExceptionHandler"/> implementations.
        /// </summary>
        /// <param name="factory">The delegate that is invoked to create the <see cref="HyperCoreServiceHost"/> object to wrap.</param>
        /// <param name="timeoutExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when a <see cref="TimeoutException"/> is thrown.</param>
        /// <param name="communicationExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when a <see cref="CommunicationException"/> is thrown.</param>
        /// <param name="genericExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when an <see cref="Exception"/> is thrown that is not a <see cref="TimeoutException"/> or a <see cref="CommunicationException"/>.</param>
        public HyperServiceHostContainer(Func<HyperCoreServiceHost> factory,
                           IServiceHostExceptionHandler timeoutExceptionHandler,
                           IServiceHostExceptionHandler communicationExceptionHandler,
                           IServiceHostExceptionHandler genericExceptionHandler)
            : this(new ServiceHostFactoryMethodWrapper(factory), timeoutExceptionHandler, communicationExceptionHandler, genericExceptionHandler)
        { }

        /// <summary>
        /// Initializes an instance of <see cref="HyperServiceHostContainer"/> with the specified <see cref="IServiceHostFactory"/> and <see cref="IServiceHostExceptionHandler"/> implementations.
        /// </summary>
        /// <param name="hostFactory">The <see cref="IServiceHostFactory"/> that is used to create the <see cref="HyperCoreServiceHost"/> object to wrap.</param>
        /// <param name="exceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when any is thrown.</param>
        public HyperServiceHostContainer(IServiceHostFactory hostFactory, IServiceHostExceptionHandler exceptionHandler)
            : this(hostFactory, exceptionHandler, exceptionHandler, exceptionHandler)
        { }

        /// <summary>
        /// Initializes an instance of <see cref="HyperServiceHostContainer"/> with the specified <see cref="IServiceHostFactory"/> and <see cref="IServiceHostExceptionHandler"/> implementations.
        /// </summary>
        /// <param name="hostFactory">The <see cref="IServiceHostFactory"/> that is used to create the <see cref="HyperCoreServiceHost"/> object to wrap.</param>
        /// <param name="timeoutExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when a <see cref="TimeoutException"/> is thrown.</param>
        /// <param name="communicationExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when a <see cref="CommunicationException"/> is thrown.</param>
        /// <param name="genericExceptionHandler">The <see cref="IServiceHostExceptionHandler"/> implementation to use when an <see cref="Exception"/> is thrown that is not a <see cref="TimeoutException"/> or a <see cref="CommunicationException"/>.</param>
        public HyperServiceHostContainer(IServiceHostFactory hostFactory, 
                           IServiceHostExceptionHandler timeoutExceptionHandler,
                           IServiceHostExceptionHandler communicationExceptionHandler,
                           IServiceHostExceptionHandler genericExceptionHandler)
        {
            _hostFactory = hostFactory ?? throw new ArgumentNullException(nameof(hostFactory));
            _timeoutExceptionHandler = timeoutExceptionHandler ?? throw new ArgumentNullException(nameof(timeoutExceptionHandler));
            _communicationExceptionHandler = communicationExceptionHandler ?? throw new ArgumentNullException(nameof(communicationExceptionHandler));
            _genericExceptionHandler = genericExceptionHandler ?? throw new ArgumentNullException(nameof(genericExceptionHandler));

            // By default, this property is true, making the container a one stop shop for ServiceHost management. Users can turn the feature off if they have to though.
            DisposeServiceOnStop = true;
        }

        /// <summary>
        /// Creates the <see cref="HyperCoreServiceHost"/> if it does not exist and calls its Open() method. Exception handling is
        /// delegated to the <see cref="IServiceHostExceptionHandler"/> implementations specified in the constructor.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            var startedSuccessfully = false;
            try
            {
                _host ??= _hostFactory.Create();
                
                _host.Open();
                startedSuccessfully = true;
            }
            catch (TimeoutException exTimeout)
            {
                _timeoutExceptionHandler.HandleException(exTimeout);
            }
            catch (CommunicationException exCommunication)
            {
                _communicationExceptionHandler.HandleException(exCommunication);
            }
            catch (Exception ex)
            {
                _genericExceptionHandler.HandleException(ex);
            }

            return startedSuccessfully;
        }

        /// <summary>
        /// Calls the <see cref="HyperCoreServiceHost.Abort()"/> method if it is in the <see cref="CommunicationState.Faulted"/> state.
        /// Otherwise, calls the <see cref="HyperCoreServiceHost.Close()"/> method instead.
        /// Calls <see cref="IDisposable.Dispose()"/> on the hosted service if it implements <see cref="IDisposable"/>.
        /// Exception handling is delegated to the <see cref="IServiceHostExceptionHandler"/> implementations specified in the constructor.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (_host != null)
                {
                    if (_host.State == CommunicationState.Faulted)
                        _host.Abort();
                    else
                        _host.Close();

                    _host = null;
                }
            }
            catch (TimeoutException exTimeout)
            {
                _timeoutExceptionHandler?.HandleException(exTimeout);
            }
            catch (CommunicationException exCommunication)
            {
                _communicationExceptionHandler?.HandleException(exCommunication);
            }
            catch (Exception ex)
            {
                _genericExceptionHandler?.HandleException(ex);
            }
            finally
            {
                // Dispose of our service if applicable
                if (DisposeServiceOnStop)
                    (_host?.SingletonInstance as IDisposable)?.Dispose();
            }
        }
    }
}
