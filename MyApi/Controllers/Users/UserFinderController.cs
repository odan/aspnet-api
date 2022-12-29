namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.User.Service;

[ApiController]
public class UserFinderController : Controller
{
    private readonly UserFinder userFinder;

    public UserFinderController(UserFinder userFinder)
    {
        this.userFinder = userFinder;
    }

    [Route("api/users")]
    [HttpGet()]
    public object FindUsers()
    {
        var users = this.userFinder.FindAllUsers();

        // Todo: Map domain objects to view data objects
        foreach (var user in users)
        {
            // ...
        }

        return new
        {
            users
        };
    }
}

