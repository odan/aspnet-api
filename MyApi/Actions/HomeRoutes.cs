namespace MyApi.Actions;

using MyApi.Actions.Home;

// Extension
public static class HomeRoutes
{
    public static IEndpointRouteBuilder MapHomeRoutes(this IEndpointRouteBuilder route)
    {
        route.MapGet("/", HomeAction.Get);

        return route;
    }
}
