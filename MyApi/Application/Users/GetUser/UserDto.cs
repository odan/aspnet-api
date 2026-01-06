namespace MyApi.Application.Users.GetUser;

// Read model (DTO)
public sealed class UserDto
{
    public int Id { get; set; }

    public string? Username { get; set; }
}