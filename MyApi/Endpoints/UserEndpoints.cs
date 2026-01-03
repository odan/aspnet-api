namespace MyApi.Endpoints;

using MyApi.Controllers.Users.CreateUser;
using MyApi.Controllers.Users.GetUser;
using MyApi.Controllers.Users.SearchUsers;

// Extension
public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapApiUserEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/users").WithTags("Users");

        group.MapGet("/", SearchUserController.Handle)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id}", GetUserController.Handle).WithName("GetUserById")
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/", CreateUserController.Handle)
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return route;
    }
}