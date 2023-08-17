using HyperSoa.Service;
using HyperSoa.Service.Configuration;
using Microsoft.Extensions.Hosting;

namespace HyperSoa.ServiceHosting
{
    public class HostedHyperNodeService : IHostedService
    {
        private readonly IHyperNodeConfigurationProvider _configProvider;
        private readonly IServiceProvider _serviceProvider;

        public HostedHyperNodeService(IHyperNodeConfigurationProvider configProvider, IServiceProvider serviceProvider)
        {
            _configProvider = configProvider;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create and configure here just in case we haven't done it yet
            HyperNodeService.CreateAndConfigure(_configProvider, _serviceProvider);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            HyperNodeService.Instance.Cancel();
            HyperNodeService.Instance.WaitAllChildTasks(cancellationToken);
            
            return Task.CompletedTask;
        }
    }
}
