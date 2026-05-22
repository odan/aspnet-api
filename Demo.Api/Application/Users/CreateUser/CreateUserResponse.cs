using System.Text.Json.Serialization;

namespace Demo.Api.Application.Users.CreateUser;

public sealed class CreateUserResponse
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}