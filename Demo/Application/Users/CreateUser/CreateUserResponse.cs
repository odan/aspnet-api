using System.Text.Json.Serialization;

namespace Demo.Application.Users.CreateUser;

public sealed class CreateUserResponse
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}