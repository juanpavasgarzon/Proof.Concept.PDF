using System.Reflection;
using File.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace File.Api.Extensions;

/// <summary>
/// Extension methods for adding and mapping endpoints in the application.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Adds all endpoints from the specified assembly.
    /// </summary>
    /// <param name="services">Reference to the service collection</param>
    /// <param name="assembly">The assembly to scan for endpoints</param>
    /// <returns>Reference to the service collection</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all endpoints to the application.
    /// </summary>
    /// <param name="app">Reference to the application builder</param>
    /// <param name="routeGroupBuilder">The route group builder</param>
    /// <returns>Returns the application builder</returns>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}