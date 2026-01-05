namespace MyApi.Controllers.Users.SearchUsers;

public sealed class SearchUsersResponse
{
    public List<SearchUsersResponseUser> Users { get; set; } = [];
}

public class SearchUsersResponseUser
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}