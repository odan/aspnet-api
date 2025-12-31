namespace MyApi.Application.Users.FindUser;

// Read Model / DTO
public sealed class UserListItem
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}