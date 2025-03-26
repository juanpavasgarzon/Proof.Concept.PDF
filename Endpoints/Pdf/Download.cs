using System.Collections.Concurrent;

namespace File.Api.Endpoints.Pdf;

internal sealed class Download : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the application.
    /// </summary>
    /// <param name="app">Reference to the endpoint route builder</param>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/pdf/{id}/download", HandleRequest)
            .WithName("Download-ByLink")
            .WithDescription("Download a file by its unique identifier.")
            .WithSummary("Download a PDF by ID.")
            .WithTags(Tags.Pdf);
    }

    /// <summary>
    /// Handles the request to get a file by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns the result of the request</returns>
    private static async Task<IResult> HandleRequest(
        string id,
        ConcurrentDictionary<string, Models.Pdf> dictionary,
        CancellationToken cancellationToken)
    {
        if (!dictionary.TryGetValue(id, out var record))
        {
            return Results.NotFound();
        }

        if (string.IsNullOrEmpty(record.Link))
        {
            return Results.BadRequest("Link cannot be null or empty.");
        }

        if (!System.IO.File.Exists(record.Link))
        {
            return Results.NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(record.Link, cancellationToken);

        return Results.File(fileBytes, "application/pdf", $"{DateTime.Now}.pdf");
    }
}