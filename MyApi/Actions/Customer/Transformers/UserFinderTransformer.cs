namespace MyApi.Actions.Customer.Transformers;

using MyApi.Domain.Customer.Data;

public sealed class UserFinderTransformer
{
    public static object Transform(IEnumerable<Customer> customers)
    {
        foreach (var customer in customers)
        {
            // ...
        }

        return new
        {
            customers
        };
    }
}
