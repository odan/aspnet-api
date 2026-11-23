using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Demo.Application.Users.UpdateUser;

public sealed class UpdateUserRequest
{
    [JsonPropertyName("username")]
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(45, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 45 characters.")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "E-Mail must be a valid email address.")]
    public string? Email { get; set; }
}