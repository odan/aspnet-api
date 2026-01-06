namespace MyApi.Application.Users.FindUser;

public sealed class SearchUsersResult
{
    public List<SearchUsersResultUser> Users { get; set; } = [];
}

public class SearchUsersResultUser
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}