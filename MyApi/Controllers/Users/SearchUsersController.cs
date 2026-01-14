namespace MyApi.Controllers.Users;

using MyApi.Application.Users.FindUser;

public static class SearchUsersController
{
    public static async Task<FindUsersResult> Invoke(FindUsersHandler userFinder)
    {
        var result = await userFinder.FindAllUsers();

        return result;
    }
}