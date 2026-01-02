namespace MyApi.Controllers.Users.SearchUsers;

using MyApi.Application.Users.FindUser;

public static class SearchUserController
{
    public static async Task<SearchUsersResponse> Handle(UserFinder userFinder)
    {
        var users = await userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model
        return new SearchUsersResponse
        {
            Users = users.Select(user => new SearchUsersResponseUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            }).ToList()
        };
    }
}