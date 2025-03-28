using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Api.Models;

namespace File.Api.Services;

internal sealed class PublishPdfGenerationService(
    Channel<PdfGenerationJob> channel,
    ConcurrentDictionary<string, Pdf> dictionary
)
{
    /// <summary>
    /// Publishes a new PDF generation job.
    /// </summary>
    /// <param name="name">The name of the file</param>
    /// <param name="file">The file to publish</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The id of the job</returns>
    public async Task<string> PublishAsync(string name, IFormFile file, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid().ToString();

        await using var stream = file.OpenReadStream();

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var data = memoryStream.ToArray();

        var job = new PdfGenerationJob(id, name, data);
        await channel.Writer.WriteAsync(job, cancellationToken);

        dictionary[id] = new Pdf(name, PdfGenerationStatus.Queued);

        return id;
    }
}