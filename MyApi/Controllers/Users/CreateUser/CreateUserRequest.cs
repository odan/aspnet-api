namespace MyApi.Controllers.Users.CreateUser;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public sealed class CreateUserRequest
{
    [JsonPropertyName("username")]
    [Required]
    [StringLength(45, MinimumLength = 3)]
    public string? Username { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [MinLength(8)]
    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    public string Role { get; set; } = "User"; // Default value
}