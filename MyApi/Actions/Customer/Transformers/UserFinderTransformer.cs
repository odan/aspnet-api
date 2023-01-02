namespace MyApi.Actions.Customer.Transformers;

using MyApi.Domain.Customer.Data;

public class UserFinderTransformer
{
    public static object transform(IEnumerable<Customer> customers)
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
