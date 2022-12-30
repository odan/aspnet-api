
namespace MyApi.Domain.User.Data;

public class UserCreatorParameter
{
    public string Username { get; set; } = "";

    public DateTime DateOfBirth { get; set; } = new DateTime(1970, 1, 1);
}
