using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Api.Models;
using File.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace File.Api.Endpoints.Pdf;

internal sealed class Create : IEndpoint
{
    private sealed record Request(string Name, IFormFile File);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/pdf/json", HandleRequest)
            .WithName("Create")
            .WithDescription("Create a new file.")
            .WithSummary("Creates a new PDF.")
            .DisableAntiforgery()
            .WithTags(Tags.PDF);
    }

    private static async Task<IResult> HandleRequest(
        [FromForm] Request request,
        Channel<PdfGenerationJob> channel,
        ConcurrentDictionary<string, PdfGenerationStatus> statuses,
        UrlResolverService linkGenerator,
        CancellationToken cancellationToken)
    {
        if (request.File.ContentType != "application/json")
        {
            return Results.BadRequest(new { Error = "Invalid file type provided. Must be application/json." });
        }

        var id = Guid.NewGuid().ToString();

        await using var stream = request.File.OpenReadStream();

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var data = memoryStream.ToArray();

        var job = new PdfGenerationJob(id, request.Name, data);
        await channel.Writer.WriteAsync(job, cancellationToken);

        statuses[id] = PdfGenerationStatus.Queued;

        var url = linkGenerator.GetUriByName("Get-ById", new { id });

        return Results.Accepted(url, new { Id = id, Status = PdfGenerationStatus.Queued });
    }
}