namespace MyApi.Controllers.User;

using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers.User.ViewModels;
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

        return UserReaderViewModel.FromUser(user);

    }
}

