namespace MyApi.Controllers.User.Transformers;

using MyApi.Domain.User.Data;

public class UserReaderTransformer
{
    public static object transform(User user)
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
