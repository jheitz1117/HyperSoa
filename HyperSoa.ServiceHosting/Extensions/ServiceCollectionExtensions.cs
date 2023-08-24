using HyperSoa.Contracts;
using HyperSoa.Service;
using HyperSoa.Service.Configuration;
using HyperSoa.ServiceHosting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace HyperSoa.ServiceHosting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHyperNodeServiceHosting<TServiceConfig>(this IServiceCollection services, IConfiguration namedConfigurationSection)
            where TServiceConfig : class, IHyperNodeConfigurationProvider
        {
            services.AddTransient<IHyperNodeConfigurationProvider, TServiceConfig>();
            services.AddSingleton<IHyperNodeService, HyperNodeService>();
            services.AddHostedService<HostedHyperNodeService>();

            services.Configure<HyperNodeHostOptions>(
                namedConfigurationSection
            ).TryAddEnumerable(
                ServiceDescriptor.Singleton<IValidateOptions<HyperNodeHostOptions>, HyperNodeHostOptionsValidator>()
            );
            services.AddSingleton<IHyperNodeServiceHost, HyperNodeServiceHost>();
            services.AddHostedService<HostedListenerService>();
        }
    }
}
