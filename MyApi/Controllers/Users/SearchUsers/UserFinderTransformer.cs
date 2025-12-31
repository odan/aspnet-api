namespace MyApi.Controllers.Users.SearchUsers;

using MyApi.Application.Users.FindUser;

public static class UserFinderTransformer
{
    public static SearchUsersResponse Transform(List<UserListItem> users)
    {
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