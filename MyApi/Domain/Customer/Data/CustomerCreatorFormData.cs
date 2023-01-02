
namespace MyApi.Domain.Customer.Data;

using System.Text.Json.Serialization;

public class CustomerCreatorFormData
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("date_of_birth")]
    public string DateOfBirth { get; set; } = "";
}
