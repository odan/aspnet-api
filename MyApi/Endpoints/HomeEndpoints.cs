using MyApi.Controllers.Home;

namespace MyApi.Endpoints;

// Extension
public static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/", HomeController.Handle).WithTags("Home");

        return route;
    }
}