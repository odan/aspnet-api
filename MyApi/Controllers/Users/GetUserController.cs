namespace MyApi.Controllers.Users;

using MyApi.Application.Users.GetUser;

public static class GetUserController
{
    public static async Task<GetUserResult> Handle(GetUserHandler userReader, int id)
    {
        var result = await userReader.GetUser(id);

        return result;

    }
}