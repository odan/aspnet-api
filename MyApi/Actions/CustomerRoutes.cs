namespace MyApi.Actions;

using Microsoft.AspNetCore.Mvc;
using MyApi.Actions.Customer;
using MyApi.Domain.Customer.Data;

// Extension
public static class CustomerRoutes
{
    public static IEndpointRouteBuilder MapApiCustomerRoutes(this IEndpointRouteBuilder route)
    {
        route.MapGet("/customers", (CustomerFinderAction action)
            => action.FindUsers());

        route.MapGet("/customers/{id}", (CustomerReaderAction action, int id)
            => action.GetUser(id));

        route.MapPost("/customers", (
           CustomerCreatorAction action,
           [FromBody] CustomerCreatorFormData data)
           => action.CreateUser(data)
        );

        return route;
    }
}
