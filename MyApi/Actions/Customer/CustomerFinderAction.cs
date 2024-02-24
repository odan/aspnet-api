namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public sealed class CustomerFinderAction(CustomerFinder userFinder)
{
    private readonly CustomerFinder _userFinder = userFinder;

    public object FindUsers()
    {
        var users = _userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model or
        // (weakly typed) view data.
        return Results.Ok(UserFinderTransformer.Transform(users));
    }
}
