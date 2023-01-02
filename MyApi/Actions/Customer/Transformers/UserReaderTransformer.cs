namespace MyApi.Actions.Customer.Transformers;

using MyApi.Domain.Customer.Data;

public class UserReaderTransformer
{
    public static object transform(Customer user)
    {
        return new
        {
            user = new
            {
                user_id = user.Id,
                username = user.Username,
            }
        };
    }
}
