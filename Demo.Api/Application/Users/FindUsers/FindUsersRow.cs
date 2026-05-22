namespace MyApi.Application.Users.FindUser;

// A result projection: a small read-side data shape 
public sealed class FindUsersRow
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}