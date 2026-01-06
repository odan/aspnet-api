namespace MyApi.Controllers.Users;

using MyApi.Application.Users.FindUser;

public static class SearchUserController
{
    public static async Task<FindUsersResult> Handle(FindUsersHandler userFinder)
    {
        var result = await userFinder.FindAllUsers();

        // ....

        return result;
    }
}