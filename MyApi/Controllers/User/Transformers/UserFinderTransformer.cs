namespace MyApi.Controllers.User.Transformers;

using MyApi.Domain.User.Data;

public class UserFinderTransformer
{
    public static object transform(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            // ...
        }

        return new
        {
            users
        };
    }
}
