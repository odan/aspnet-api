namespace MyApi.Controllers.Users.SearchUsers;

using MyApi.Application.Users.FindUser;

public static class SearchUserController
{
    public static async Task<SearchUsersResponse> Handle(FindUsersQueryHandler userFinder)
    {
        var users = await userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model
        var result = new List<SearchUsersResponseUser>(users.Count);

        foreach (var user in users)
        {
            result.Add(new SearchUsersResponseUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            });
        }

        return new SearchUsersResponse { Users = result };
    }
}