using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaggieVision.Host;

/// <summary>Defines the inert Windows host composition for the WP-00 foundation.</summary>
public static class HostComposition
{
    public const string WindowsServiceName = "MaggieVision Host";

    /// <summary>Builds the service host without enabling media or network capabilities.</summary>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseWindowsService(options => options.ServiceName = WindowsServiceName)
            .ConfigureServices(services => services.AddHostedService<InertHostWorker>());
}
