using MaggieVision.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaggieVision.Host.Tests;

public sealed class HostCompositionTests
{
    [Fact]
    public async Task Host_starts_and_stops_with_only_the_inert_worker_registered()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var host = HostComposition.CreateHostBuilder(Array.Empty<string>()).Build();

        var hostedServices = host.Services.GetServices<IHostedService>();
        Assert.Single(hostedServices);

        await host.StartAsync(timeout.Token);
        await host.StopAsync(timeout.Token);
    }
}
