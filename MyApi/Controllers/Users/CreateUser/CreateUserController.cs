namespace MyApi.Controllers.Users.CreateUser;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.GetUser;

public static class CreateUserController
{
    public static async Task<IResult> Handle(
        UserCreator userCreator,
        [FromBody] CreateUserRequest request
    )
    {
        var userId = await userCreator.CreateUser(request);

        return Results.CreatedAtRoute(
            "GetUserById",
            new { id = userId },
            new CreateUserResponse { UserId = userId }
        );
    }

}