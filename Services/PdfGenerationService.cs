using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using File.Api.Models;

namespace File.Api.Services;

internal sealed class PdfGenerationService(
    ILogger<PdfGenerationService> logger,
    Channel<PdfGenerationJob> channel,
    ConcurrentDictionary<string, PdfGenerationStatus> statuses
) : BackgroundService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfGenerationService"/> class.
    /// </summary>
    /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await ProcessJobAsync(job, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while processing the job");
            }
        }
    }

    /// <summary>
    /// Processes the job asynchronously.
    /// </summary>
    /// <param name="job">The job to process</param>
    /// <param name="cancellationToken">The cancellation token</param>
    private async Task ProcessJobAsync(PdfGenerationJob job, CancellationToken cancellationToken)
    {
        statuses[job.Id] = PdfGenerationStatus.Processing;

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            var content = Encoding.UTF8.GetString(job.Data);

            logger.LogInformation("Processing job {Id} with json {Json}", job.Id, content);

            statuses[job.Id] = PdfGenerationStatus.Completed;
        }
        catch (Exception)
        {
            statuses[job.Id] = PdfGenerationStatus.Failed;
            throw;
        }
    }
}