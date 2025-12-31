namespace MyApi.Controllers.Users.CreateUser;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.GetUser;

public static class CreateUserController
{
    public static async Task<IResult> Handle(
        UserCreator userCreator,
        UserRequestAdapter mapper,
        [FromBody] CreateUserRequest form
    )
    {
        // Todo: Move validation to application service
        var parameter = mapper.Map(form);

        var userId = await userCreator.CreateUser(parameter);

        return Results.CreatedAtRoute(
            "GetUserById",
            new { id = userId },
            new CreateUserResponse { UserId = userId }
        );
    }

}