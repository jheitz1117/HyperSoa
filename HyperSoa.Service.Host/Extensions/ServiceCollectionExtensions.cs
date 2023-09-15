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
        public static IServiceCollection AddHyperNodeServiceHosting(this IServiceCollection services, Func<IServiceProvider, IHyperNodeConfigurationProvider> configProviderFactory, IConfiguration hostConfigSection)
        {
            services.AddTransient(configProviderFactory);
            services.AddSingleton<IHyperNodeService, HyperNodeService>();
            services.AddHostedService<HostedHyperNodeService>();

            services.Configure<HyperNodeHostConfiguration>(
                hostConfigSection
            ).TryAddEnumerable(
                ServiceDescriptor.Singleton<IValidateOptions<HyperNodeHostConfiguration>, HyperNodeHostConfigurationValidator>()
            );
            services.AddSingleton<IHyperNodeServiceHost, HyperNodeServiceHost>();
            services.AddHostedService<HostedListenerService>();

            return services;
        }

        public static IServiceCollection AddHyperNodeServiceHosting(this IServiceCollection services, IConfiguration serviceConfigSection, IConfiguration hostConfigSection)
        {
            return services.AddHyperNodeServiceHosting(
                _ => new InMemoryHyperNodeConfigurationProvider(
                    serviceConfigSection.Get<HyperNodeConfiguration>() ?? throw new InvalidOperationException($"Unable to deserialize {nameof(HyperNodeConfiguration)} from configuration.")
                ),
                hostConfigSection
            );
        }

        public static IServiceCollection AddHyperNodeServiceHosting(this IServiceCollection services, IConfiguration configuration, bool configurationIsScoped = false)
        {
            return services.AddHyperNodeServiceHosting(
                configuration.GetSection(
                    configurationIsScoped
                        ? HyperNodeConfiguration.ScopedConfigurationSectionName
                        : HyperNodeConfiguration.ConfigurationSectionName
                ),
                configuration.GetSection(
                    configurationIsScoped
                        ? HyperNodeHostConfiguration.ScopedConfigurationSectionName
                        : HyperNodeHostConfiguration.ConfigurationSectionName
                )
            );
        }
    }
}
