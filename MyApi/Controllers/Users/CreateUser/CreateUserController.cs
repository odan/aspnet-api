namespace MyApi.Controllers.Users.CreateUser;

using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;

public static class CreateUserController
{
    public static async Task<IResult> Handle(
        UserCreator userCreator,
        [FromBody] CreateUserRequest request
    )
    {
        // Map request to command
        var command = new CreateUserCommand
        {
            Username = request.Username ?? string.Empty,
            Email = request.Email ?? string.Empty,
            Password = request.Password ?? string.Empty,
            FirstName = request.FirstName ?? string.Empty,
            LastName = request.LastName ?? string.Empty,
            DateOfBirth = request.DateOfBirth,
            Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role
        };

        // Execute use case
        var userId = await userCreator.CreateUser(command);

        // Return response
        return Results.CreatedAtRoute(
            "GetUserById",
            new { id = userId },
            new CreateUserResponse { UserId = userId }
        );
    }

}