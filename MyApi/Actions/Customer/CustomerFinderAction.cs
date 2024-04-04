namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public static class CustomerFinderAction
{
    public static object FindUsers(CustomerFinder userFinder)
    {
        var users = userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model or
        // (weakly typed) view data.
        return Results.Ok(UserFinderTransformer.Transform(users));
    }
}