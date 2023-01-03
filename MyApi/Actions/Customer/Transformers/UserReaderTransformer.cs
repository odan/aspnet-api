namespace MyApi.Actions.Customer.Transformers;

using MyApi.Domain.Customer.Data;

public sealed class UserReaderTransformer
{
    public static object Transform(Customer user)
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
