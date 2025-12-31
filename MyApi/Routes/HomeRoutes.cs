using MyApi.Controllers.Home;

namespace MyApi.Routes;

// Extension
public static class HomeRoutes
{
    public static IEndpointRouteBuilder MapHomeRoutes(this IEndpointRouteBuilder route)
    {
        route.MapGet("/", HomeController.Handle).WithTags("Home");

        return route;
    }
}