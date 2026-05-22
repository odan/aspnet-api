namespace MyApi.Application.Users.GetUser;

// Read model (DTO)
public sealed class GetUserByIdRow
{
    public int Id { get; set; }

    public string? Username { get; set; }
}