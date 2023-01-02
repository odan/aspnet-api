namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public class CustomerReaderAction
{
    private readonly CustomerReader userReader;

    public CustomerReaderAction(CustomerReader userReader)
    {
        this.userReader = userReader;
    }

    public object GetUser(int id)
    {
        var user = this.userReader.ReadUser(id);

        return Results.Ok(UserReaderTransformer.transform(user));

    }
}

