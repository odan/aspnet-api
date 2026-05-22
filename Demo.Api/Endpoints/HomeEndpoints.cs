namespace Demo.Endpoints;

using Demo.Api.Application.Home;

// Extension
public static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/", async (HomeHandler handler) => await handler.Invoke()).WithTags("Home");

        return route;
    }
}
