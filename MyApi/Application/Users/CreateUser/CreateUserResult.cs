using System.Text.Json.Serialization;

namespace MyApi.Application.Users.CreateUser;

public sealed class CreateUserResult
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}