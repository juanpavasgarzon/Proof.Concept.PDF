using System.Collections.Concurrent;
using File.Api.Models;

namespace File.Api.Endpoints.Pdf;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/pdf/{id}", HandleRequest)
            .WithName("Get-ById")
            .WithDescription("Get a file by its unique identifier.")
            .WithSummary("Get a PDF by ID.")
            .WithTags(Tags.PDF);
    }

    private static IResult HandleRequest(
        string id,
        ILogger<GetById> logger,
        ConcurrentDictionary<string, PdfGenerationStatus> statuses,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting file by id");

        if (!statuses.TryGetValue(id, out var status))
        {
            return Results.NotFound();
        }

        var response = new { Id = id, Status = status, Link = string.Empty };

        if (status == PdfGenerationStatus.Completed)
        {
            response = response with { Link = $"https://localhost:5001/pdf/{id}/download" };
        }

        return Results.Ok(response);
    }
}