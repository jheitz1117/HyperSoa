using HyperSoa.Contracts;
using HyperSoa.Service;
using Microsoft.Extensions.Hosting;

namespace HyperSoa.ServiceHosting
{
    public class HostedHyperNodeService : IHostedService
    {
        private readonly HyperNodeService _cancellableService;

        public HostedHyperNodeService(IHyperNodeService serviceInstance)
        {
            if (serviceInstance is not HyperNodeService cancellableService)
                throw new ArgumentException($"Implementation must be {typeof(HyperNodeService)}.", nameof(serviceInstance));
                
            _cancellableService = cancellableService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellableService.Cancel();
            _cancellableService.WaitAllChildTasks(cancellationToken);
            
            await Task.CompletedTask;
        }
    }
}
