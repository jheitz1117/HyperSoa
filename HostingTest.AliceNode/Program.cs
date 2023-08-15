using HyperSoa.Service;
using HyperSoa.Service.Configuration.Json;
using HyperSoa.ServiceHosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IHyperNodeConfigurationProvider, JsonHyperNodeConfigurationProvider>();
builder.Services.AddHostedService<HostedHyperNodeService>();

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

        Console.WriteLine("Press any key to stop service...");
        Console.ReadKey();
        Console.WriteLine("Stopping service...");
        tokenSource.Cancel();

        await host.WaitForShutdownAsync(tokenSource.Token);
        
        Console.WriteLine("Done.");
        Thread.Sleep(1000);
    }
}

