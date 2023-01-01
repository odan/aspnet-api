namespace MyApi.Controllers.User.ViewModels;

using MyApi.Domain.User.Data;

public class UserReaderViewModel
{
    public static object FromUser(User user)
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
