
namespace MyApi.Domain.User.Data;

using System.Text.Json.Serialization;

public class UserCreatorForm
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("date_of_birth")]
    public string DateOfBirth { get; set; } = "";
}
