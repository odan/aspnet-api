namespace MyApi.Endpoints;

using MyApi.Application.Users.CreateUser;
using MyApi.Controllers.Users;

// Extension
public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapApiUserEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/users").WithTags("Users");

        group.MapGet("/", SearchUsersController.Invoke)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id}", GetUserController.Invoke)
            .WithName(nameof(GetUserController))
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/", CreateUserController.Invoke)
            .Produces<CreateUserResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return route;
    }
}