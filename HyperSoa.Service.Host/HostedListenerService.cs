using Microsoft.Extensions.Hosting;

namespace HyperSoa.Service.Host
{
    public class HostedListenerService : IHostedService
    {
        private readonly IHyperNodeServiceHost _serviceHost;

        public HostedListenerService(IHyperNodeServiceHost serviceHost)
        {
            _serviceHost = serviceHost;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var channel in _serviceHost.GetChannels())
            {
                channel.Open();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var channel in _serviceHost.GetChannels())
            {
                channel.Close();
            }

            return Task.CompletedTask;
        }
    }
}
