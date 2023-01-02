namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public class CustomerFinderAction
{
    private readonly CustomerFinder userFinder;

    public CustomerFinderAction(CustomerFinder userFinder)
    {
        this.userFinder = userFinder;
    }

    public object FindUsers()
    {
        var users = this.userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model or
        // (weakly typed) view data.
        return Results.Ok(UserFinderTransformer.transform(users));
    }
}
