using HyperSoa.Contracts;
using HyperSoa.Service;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.Configuration.Json;
using HyperSoa.ServiceHosting;
using HyperSoa.ServiceHosting.Configuration;
using HyperSoa.ServiceHosting.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Add HyperNodeService dependencies
builder.Services.AddTransient<IHyperNodeConfigurationProvider, JsonHyperNodeConfigurationProvider>();
builder.Services.AddSingleton<IHyperNodeService>(
    serviceProvider =>
    {
        HyperNodeService.CreateAndConfigure(
            serviceProvider.GetRequiredService<IHyperNodeConfigurationProvider>()
        );

        return HyperNodeService.Instance;
    }
);
builder.Services.AddHostedService<HostedHyperNodeService>();

// Add HyperNodeHost dependencies
builder.Services.AddTransient<IHyperNodeHostConfigurationProvider, JsonHyperNodeHostConfigurationProvider>();
builder.Services.AddSingleton<IHyperNodeServiceHost, HyperNodeServiceHost>();
builder.Services.AddHostedService<HostedListenerService>();

try
{
    using (var host = builder.Build())
    {
        // TODO: Would like to use DI to get loggers inside hypernode service and host as well
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        using (var tokenSource = new CancellationTokenSource())
        {
            await host.StartAsync(tokenSource.Token);

            var serviceHost = host.Services.GetRequiredService<IHyperNodeServiceHost>();

            Console.WriteLine("Service started and is listening on the following addresses:");
            foreach (var channel in serviceHost.GetChannels())
            {
                if (channel.Endpoints?.Any() ?? false)
                {
                    foreach (var endpoint in channel.Endpoints)
                    {
                        Console.WriteLine("    " + endpoint);
                    }    
                }
            }

            Console.WriteLine("Press any key to stop service...");
            Console.ReadKey();
            Console.WriteLine("Stopping service...");
            tokenSource.Cancel();

            await host.WaitForShutdownAsync(tokenSource.Token);
        
            Console.WriteLine("Done.");
            Thread.Sleep(1000);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.ReadKey();
}
