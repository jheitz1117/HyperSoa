﻿using HyperSoa.Service.Host;
using HyperSoa.Service.Host.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHyperNodeServiceHosting(builder.Configuration);

try
{
    using (var host = builder.Build())
    {
        await host.StartAsync();

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

        await host.StopAsync();
        await host.WaitForShutdownAsync();
        
        Console.WriteLine("Done.");
        await Task.Delay(1000);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.ReadKey();
}
