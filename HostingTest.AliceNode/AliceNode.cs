using HostingTest.AliceNode;
using HyperSoa.Service;
using HyperSoa.Service.Configuration.Json;
using HyperSoa.Service.Configuration.Xml;
using HyperSoa.ServiceHosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IHyperNodeConfigurationProvider, JsonHyperNodeConfigurationProvider>();
builder.Services.AddTransient<IServiceHostExceptionHandler, DefaultServiceHostExceptionHandler>();

using (var host = builder.Build())
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();

    logger.LogTrace("Trace Message");
    logger.LogDebug("Debug Message");
    logger.LogInformation("Info Message");
    logger.LogWarning("Warning Message");
    logger.LogError("Error Message");
    logger.LogCritical("Critical Message");

    using (var tokenSource = new CancellationTokenSource())
    {
        await host.StartAsync(tokenSource.Token);

        var configProvider = host.Services.GetRequiredService<IHyperNodeConfigurationProvider>();
        var hostExHandler = host.Services.GetService<IServiceHostExceptionHandler>();
        var container = new HyperServiceHostContainer(
            () =>
            {
                // TODO: This will configure the node using the app.config file; presumably we want to replace this with a JSON provider that can read the appsettings.json file instead.
                //HyperNodeService.CreateAndConfigure(
                //    new HyperNodeSectionConfigurationProvider()
                //);
                HyperNodeService.CreateAndConfigure(configProvider);

                // TODO: Speaking of appsettings.json, would like to take advantage of default behavior in .NET core and see if we can use DI to get a reference to an IConfiguration object we can use to pull all config values
                //IConfiguration config; // TODO: How to use DI to get a hold of one of these?

                var serviceHost = new CancellableServiceHost(HyperNodeService.Instance);

                // When we abort, we want to cancel the service and wait for all child tasks to complete
                serviceHost.RegisterCancellationDelegate(
                    args =>
                    {
                        var cancelParams = (CancellationParams)args!;
                        HyperNodeService.Instance.Cancel();
                        HyperNodeService.Instance.WaitAllChildTasks(cancelParams.MillisecondsTimeout, cancelParams.Token);
                    },
                    new CancellationParams
                    {
                        // TODO: Write about how these settings can be pulled from the app.config settings as a way to customize how long the service will wait after cancellation before proceeding to force a close.
                        MillisecondsTimeout = 100,
                        Timeout = TimeSpan.FromMilliseconds(2000),
                        Token = tokenSource.Token
                    }
                );

                return serviceHost;
            },
            hostExHandler
        );

        Console.WriteLine("Starting service...");
        if (!container.Start())
        {
            Console.WriteLine("Failed to start service. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Service started and is listening on the following addresses:");
        foreach (var endpoint in container.Endpoints)
        {
            Console.WriteLine($"    {endpoint}");
        }

        Console.WriteLine("Press any key to stop service...");
        Console.ReadKey();
        Console.WriteLine("Stopping service...");
        container.Stop();
        tokenSource.Cancel();

        await host.WaitForShutdownAsync(tokenSource.Token);
        
        Thread.Sleep(1000);
    }
}

