namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public sealed class CustomerReaderAction
{
    private readonly CustomerReader _userReader;

    public CustomerReaderAction(CustomerReader userReader)
    {
        _userReader = userReader;
    }

    public object GetUser(int id)
    {
        var user = _userReader.ReadUser(id);

        return Results.Ok(UserReaderTransformer.Transform(user));

    }
}

