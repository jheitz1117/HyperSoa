using HyperSoa.Service;
using HyperSoa.Service.Configuration;
using Microsoft.Extensions.Hosting;

namespace HyperSoa.ServiceHosting
{
    public class HostedHyperNodeService : IHostedService
    {
        private readonly IHyperNodeConfigurationProvider _configProvider;

        public HostedHyperNodeService(IHyperNodeConfigurationProvider configProvider)
        {
            // TODO: I think we could grab a ILogger<HyperNodeService> right here and pass to the instance
            // TODO: If we also grab the IServiceProvider itself, we could also pass it into the service so that the service can generate ILoggers for each command and attach to the command context!
            _configProvider = configProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create and configure here just in case we haven't done it yet
            HyperNodeService.CreateAndConfigure(_configProvider);

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
