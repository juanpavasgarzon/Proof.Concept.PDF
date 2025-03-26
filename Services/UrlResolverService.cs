namespace File.Api.Services;

internal sealed class UrlResolverService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UrlResolverService"/> class.
    /// </summary>
    /// <param name="name">The name of the route</param>
    /// <param name="parameters">The parameters</param>
    /// <returns>The URI</returns>
    public string? GetUriByName(string name, object parameters)
    {
        var httpContext = httpContextAccessor.HttpContext;
        return httpContext is null ? null : linkGenerator.GetUriByName(httpContext, name, parameters);
    }
}