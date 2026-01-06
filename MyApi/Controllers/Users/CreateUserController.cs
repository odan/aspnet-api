namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;

public static class CreateUserController
{
    public static async Task<IResult> Handle(
        CreateUserHandler handler,
        [FromBody] CreateUserCommand command
    )
    {
        // Execute use case
        var userId = await handler.Handle(command);

        // Return response
        return Results.CreatedAtRoute(
            "GetUserById",
            new { id = userId },
            new CreateUserResult { UserId = userId }
        );
    }

}