namespace File.Api.Services;

internal sealed class UrlResolverService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
{
    public string? GetUriByName(string name, object parameters)
    {
        var httpContext = httpContextAccessor.HttpContext;
        return httpContext is null ? null : linkGenerator.GetUriByName(httpContext, name, parameters);
    }
}