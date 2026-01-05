namespace MyApi.Controllers.Users.CreateUser;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;

public static class CreateUserController
{
    public static async Task<IResult> Handle(
        CreateUserCommandHandler handler,
        [FromBody] CreateUserCommand command
    )
    {
        // Execute use case
        var userId = await handler.Handle(command);

        // Return response
        return Results.CreatedAtRoute(
            "GetUserById",
            new { id = userId },
            new CreateUserResponse { UserId = userId }
        );
    }

}