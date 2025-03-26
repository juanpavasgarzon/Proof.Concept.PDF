namespace File.Api.Endpoints;

internal interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the application.
    /// </summary>
    /// <param name="app">Reference to the endpoint route builder</param>
    public void MapEndpoint(IEndpointRouteBuilder app);
}
