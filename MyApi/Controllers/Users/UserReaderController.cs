namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.User.Service;

[ApiController]
public class UserReaderController : Controller
{
    private readonly UserReader userReader;

    public UserReaderController(UserReader userReader)
    {
        this.userReader = userReader;
    }

    [Route("api/users/{id}")]
    [HttpGet()]
    public object GetUser(int id)
    {
        var user = this.userReader.ReadUser(id);

        return new
        {
            user = new
            {
                user_id = user.Id,
                username = user.Username,
            }
        };
    }
}

