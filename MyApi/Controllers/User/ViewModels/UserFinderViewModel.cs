namespace MyApi.Controllers.User.ViewModels;

using MyApi.Domain.User.Data;

public class UserFinderViewModel
{
    public static object FromUsers(IEnumerable<User> users)
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
