namespace MyApi.Controllers.Users.GetUser;

public record GetUserResponse
{

    public int UserId { get; set; }

    public string? UserName { get; set; }

}