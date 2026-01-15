namespace MyApi.Application.Users.FindUser;

public sealed class FindUsersResult
{
    public List<FindUsersItem> Users { get; init; } = [];
}

public sealed class FindUsersItem
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}