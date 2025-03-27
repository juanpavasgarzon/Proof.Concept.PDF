using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Api.Extensions;
using File.Api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace File.Api.Services;

internal sealed class PdfGenerationService(
    ILogger<PdfGenerationService> logger,
    Channel<PdfGenerationJob> channel,
    ConcurrentDictionary<string, Pdf> dictionary,
    IConfiguration configuration
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
                await ProcessJobAsync(job);
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
    private async Task ProcessJobAsync(PdfGenerationJob job)
    {
        var record = dictionary[job.Id];

        await Task.Delay(TimeSpan.FromSeconds(10));
        try
        {
            record.Status = PdfGenerationStatus.Processing;

            var output = $"Generated/{job.Name}_{job.Id}.pdf";
            RenderPdfService.Render(output);

            var baseUrl = new Uri(configuration.GetRequired("BaseUrl").TrimEnd('/'));
            var fullUrl = new Uri(baseUrl, $"/pdf/{job.Id}/download");

            record.Status = PdfGenerationStatus.Completed;
            record.Output = output;
            record.Link = fullUrl.ToString();
        }
        catch (Exception)
        {
            record.Status = PdfGenerationStatus.Failed;
            throw;
        }
    }
}