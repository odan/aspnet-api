namespace MyApi.Application.Users.CreateUser;

public sealed class CreateUserCommand
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string Role { get; init; } = "User";
}