using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyApi.Application.Users.CreateUser;

public sealed class CreateUserCommand
{
    [JsonPropertyName("username")]
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(45, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 45 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "E-Mail is required.")]
    [EmailAddress(ErrorMessage = "E-Mail must be a valid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string? LastName { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = "User";
}