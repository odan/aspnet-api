namespace MyApi.Controllers.User;

using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers.User.ViewModels;
using MyApi.Domain.User.Service;

[ApiController]
public class UserFinderController : Controller
{
    private readonly UserFinder userFinder;

    [ActivatorUtilitiesConstructor]
    public UserFinderController(UserFinder userFinder)
    {
        this.userFinder = userFinder;
    }

    [Route("api/users")]
    [HttpGet()]
    public object FindUsers()
    {
        var users = this.userFinder.FindAllUsers();

        // Map domain objects to (strongly typed) view model or
        // (weakly typed) view data.
        return UserFinderViewModel.FromUsers(users);
    }
}
