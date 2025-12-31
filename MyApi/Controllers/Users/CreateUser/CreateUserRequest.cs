namespace MyApi.Controllers.Users.CreateUser;

using System.Text.Json.Serialization;

public record CreateUserRequest
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public string? DateOfBirth { get; set; }
}