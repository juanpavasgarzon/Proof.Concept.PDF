using System.Collections.Concurrent;
using File.Api.Models;

namespace File.Api.Endpoints.Pdf;

internal sealed class GetById : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the application.
    /// </summary>
    /// <param name="app">Reference to the endpoint route builder</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/pdf/{id}", HandleRequest)
            .WithName("Get-ById")
            .WithDescription("Get a file by its unique identifier.")
            .WithSummary("Get a PDF by ID.")
            .WithTags(Tags.Pdf);
    }

    /// <summary>
    /// Handles the request to get a file by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the file</param>
    /// <param name="dictionary">The statuses of the PDF generation jobs</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Returns the result of the request</returns>
    private static IResult HandleRequest(
        string id,
        ConcurrentDictionary<string, Models.Pdf> dictionary,
        CancellationToken cancellationToken)
    {
        return !dictionary.TryGetValue(id, out var record)
            ? Results.NotFound()
            : Results.Ok(record.ToJson());
    }
}