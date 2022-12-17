namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.User.Service;

[ApiController]
public class UserCreatorController : Controller
{
    private readonly UserCreator userCreator;

    public UserCreatorController(UserCreator userCreator)
    {
        this.userCreator = userCreator;
    }

    [Route("api/users")]
    [HttpPost]
    public CreatedResult CreateUser()
    {
        var userId = this.userCreator.CreateUser("test");

        return this.Created("", new { user_id = userId });
    }
}

