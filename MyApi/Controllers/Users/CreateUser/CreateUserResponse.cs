using System.Text.Json.Serialization;

namespace MyApi.Controllers.Users.CreateUser;

public record CreateUserResponse
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}
