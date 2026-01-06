namespace MyApi.Controllers.Users;

using MyApi.Application.Users.GetUser;

public static class GetUserController
{
    public static async Task<GetUserResult> Handle(GetUserHandler userReader, int id)
    {
        var user = await userReader.GetUser(id);

        var response = new GetUserResult
        {
            UserId = user.Id,
            UserName = user.Username,
        };

        return response;

    }
}