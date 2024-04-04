namespace MyApi.Actions;

using MyApi.Actions.Customer;

// Extension
public static class CustomerRoutes
{
    public static IEndpointRouteBuilder MapApiCustomerRoutes(this IEndpointRouteBuilder route)
    {
        route.MapGet("/customers", CustomerFinderAction.FindUsers);

        route.MapGet("/customers/{id}", CustomerReaderAction.GetUser);

        route.MapPost("/customers", CustomerCreatorAction.CreateUser);

        return route;
    }
}
