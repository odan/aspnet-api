namespace MyApi.Actions.Customer;

using MyApi.Actions.Customer.Transformers;
using MyApi.Domain.Customer.Service;

public static class CustomerReaderAction
{
    public static object GetUser(CustomerReader userReader, int id)
    {
        var user = userReader.ReadUser(id);

        return Results.Ok(UserReaderTransformer.Transform(user));

    }
}

