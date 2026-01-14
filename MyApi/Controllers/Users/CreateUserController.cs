namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;

public static class CreateUserController
{
    public static async Task<IResult> Invoke(
        CreateUserHandler handler,
        [FromBody] CreateUserCommand command
    )
    {
        // Execute use case
        var result = await handler.Handle(command);

        // Return response
        return Results.CreatedAtRoute(
            nameof(GetUserController),
            new { id = result.UserId },
            result
        );
    }

}