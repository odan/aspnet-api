namespace MyApi.Controllers.User;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers.User.Mappers;
using MyApi.Domain.User.Data;
using MyApi.Domain.User.Service;

[ApiController]
public class UserCreatorController : Controller
{
    private readonly UserCreator userCreator;
    private readonly UserCreatorFormMapper mapper;

    public UserCreatorController(
        UserCreator userCreator,
        UserCreatorFormMapper mapper
    )
    {
        this.userCreator = userCreator;
        this.mapper = mapper;
    }

    [Route("api/users")]
    [HttpPost]
    public CreatedResult CreateUser([FromBody] JsonDocument document)
    {
        // Deserialize JSON payload to object
        // https://bit.ly/3i2ZPGc
        // https://bit.ly/3WBAAK5
        var form = JsonSerializer.Deserialize<UserCreatorForm>(
            document.RootElement.ToString()
        );

        var parameter = this.mapper.Map(form);

        var userId = this.userCreator.CreateUser(parameter);

        return this.Created("", new { user_id = userId });
    }

}

