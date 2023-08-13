namespace HyperSoa.ServiceHosting
{
    /// <summary>
    /// Defines a method to create <see cref="HyperCoreServiceHost"/> instances.
    /// </summary>
    public interface IServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="HyperCoreServiceHost"/> instance.
        /// </summary>
        /// <returns></returns>
        HyperCoreServiceHost Create();
    }
}
