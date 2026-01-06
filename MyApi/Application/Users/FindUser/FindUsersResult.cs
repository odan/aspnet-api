namespace MyApi.Application.Users.FindUser;

public sealed class FindUsersResult
{
    public List<UserSummary> Users { get; init; } = [];
}

public sealed class UserSummary
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}