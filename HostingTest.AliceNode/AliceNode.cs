using HostingTest.AliceNode;
using HyperSoa.Service;
using HyperSoa.Service.Configuration.Xml;
using HyperSoa.ServiceHosting;
using Microsoft.Extensions.Configuration;

var tokenSource = new CancellationTokenSource();

var container = new HyperServiceHostContainer(
    () =>
    {
        // TODO: This will configure the node using the app.config file; presumably we want to replace this with a JSON provider that can read the appsettings.json file instead.
        HyperNodeService.CreateAndConfigure(
            new HyperNodeSectionConfigurationProvider()
        );
        //HyperNodeService.CreateAndConfigure(
        //    new JsonHyperNodeConfigurationProvider()
        //);

        // TODO: Speaking of appsettings.json, would like to take advantage of default behavior in .NET core and see if we can use DI to get a reference to an IConfiguration object we can use to pull all config values
        //IConfiguration config; // TODO: How to use DI to get a hold of one of these?

        var host = new CancellableServiceHost(HyperNodeService.Instance);

        // When we abort, we want to cancel the service and wait for all child tasks to complete
        host.RegisterCancellationDelegate(
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

        return host;
    },
    new DefaultServiceHostExceptionHandler() // TODO: Exceptions starting up the host are not being propagated out to the console...why is that?
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

Console.WriteLine("Done.");
Thread.Sleep(1000);