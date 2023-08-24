using HyperSoa.Contracts;
using HyperSoa.Service;
using HyperSoa.Service.Configuration;
using HyperSoa.ServiceHosting;
using HyperSoa.ServiceHosting.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddHyperNodeServiceHosting<TServiceConfig, THostConfig>(this IServiceCollection services)
            where TServiceConfig : class, IHyperNodeConfigurationProvider
            where THostConfig : class, IHyperNodeHostConfigurationProvider
        {
            services.AddTransient<IHyperNodeConfigurationProvider, TServiceConfig>();
            services.AddSingleton<IHyperNodeService, HyperNodeService>();
            services.AddHostedService<HostedHyperNodeService>();

            services.AddTransient<IHyperNodeHostConfigurationProvider, THostConfig>();
            services.AddSingleton<IHyperNodeServiceHost, HyperNodeServiceHost>();
            services.AddHostedService<HostedListenerService>();
        }
    }
}
