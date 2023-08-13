namespace HyperSoa.ServiceHosting
{
    /// <summary>
    /// <see cref="IServiceHostFactory"/> implementation to wrap a factory method that creates instances of <see cref="HyperCoreServiceHost"/>.
    /// </summary>
    internal sealed class ServiceHostFactoryMethodWrapper : IServiceHostFactory
    {
        private readonly Func<HyperCoreServiceHost> _factory;

        /// <summary>
        /// Initializes an instance of <see cref="ServiceHostFactoryMethodWrapper"/> using the specified factory method.
        /// </summary>
        /// <param name="factory">The delegate that is invoked to create the <see cref="HyperCoreServiceHost"/> object.</param>
        public ServiceHostFactoryMethodWrapper(Func<HyperCoreServiceHost> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Creates an instance of <see cref="HyperCoreServiceHost"/> using the specified factory method.
        /// </summary>
        /// <returns></returns>
        public HyperCoreServiceHost Create()
        {
            return _factory();
        }
    }
}
