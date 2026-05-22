namespace Demo.Endpoints;

using Demo.Api.Application.Users.CreateUser;
using Demo.Api.Application.Users.FindUser;
using Demo.Api.Application.Users.GetUser;

// Extension
public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapApiUserEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/users").WithTags("Users");

        group.MapGet("/", async (FindUsersHandler handler) =>
            await handler.Invoke())
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id}", async (GetUserHandler handler, int id) =>
            await handler.Invoke(id))
            .WithName(GetUserHandler.RouteName)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/", async (CreateUserHandler handler, CreateUserRequest request) =>
            await handler.Invoke(request))
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return route;
    }
}
