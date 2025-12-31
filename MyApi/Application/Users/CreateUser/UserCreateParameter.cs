namespace MyApi.Application.Users.CreateUser;

public class UserCreateParameter
{
    public string Username { get; set; } = "";

    public DateTime DateOfBirth { get; set; } = new DateTime(1970, 1, 1);
}