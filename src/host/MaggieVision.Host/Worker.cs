using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MaggieVision.Host;

/// <summary>Waits for service shutdown while WP-00 capabilities remain intentionally disabled.</summary>
internal sealed class InertHostWorker(ILogger<InertHostWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "MaggieVision host started in inert WP-00 mode; discovery, media access, and listeners are disabled.");

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal service shutdown.
        }
    }
}
