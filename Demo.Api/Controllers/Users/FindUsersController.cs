namespace MyApi.Controllers.Users;

using MyApi.Application.Users.FindUser;

public static class FindUsersController
{
    public static async Task<FindUsersResponse> Invoke(FindUsersHandler userFinder)
    {
        var result = await userFinder.FindAllUsers();

        return result;
    }
}