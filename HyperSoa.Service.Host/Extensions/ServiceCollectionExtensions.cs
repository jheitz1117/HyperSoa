using HyperSoa.Contracts;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Host.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace HyperSoa.Service.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHyperNodeServiceHosting<TServiceConfig>(this IServiceCollection services, IConfiguration namedConfigurationSection)
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

            return services;
        }
    }
}
