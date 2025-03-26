using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Api.Models;
using File.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace File.Api.Endpoints.Pdf;

internal sealed class Create : IEndpoint
{
    /// <summary>
    /// The request to create a new PDF.
    /// </summary>
    /// <param name="Name">The name of the PDF.</param>
    /// <param name="File">The file to create the PDF from.</param>
    private sealed record Request(string Name, IFormFile File);

    /// <summary>
    /// Maps the endpoint.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/pdf/json", HandleRequest)
            .WithName("Create")
            .WithDescription("Create a new file.")
            .WithSummary("Creates a new PDF.")
            .DisableAntiforgery()
            .WithTags(Tags.Pdf);
    }

    /// <summary>
    /// Handles the request to create a new PDF.
    /// </summary>
    /// <param name="request">The request to create a new PDF.</param>
    /// <param name="publisher">The PDF generation service.</param>
    /// <param name="linkGenerator">The URL resolver service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the result of the request.</returns>
    private static async Task<IResult> HandleRequest(
        [FromForm] Request request,
        PublishPdfGenerationService publisher,
        UrlResolverService linkGenerator,
        CancellationToken cancellationToken)
    {
        if (request.File.ContentType != "application/json")
        {
            return Results.BadRequest(new { Error = "Invalid file type provided. Must be application/json." });
        }

        var streamId = await publisher.PublishAsync(request.Name, request.File, cancellationToken);
        var url = linkGenerator.GetUriByName("Get-ById", new { id = streamId });

        return Results.Accepted(url, new { Id = streamId, Status = PdfGenerationStatus.Queued });
    }
}