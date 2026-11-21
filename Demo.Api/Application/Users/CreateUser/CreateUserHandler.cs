namespace Demo.Api.Application.Users.CreateUser;

using Demo.Api.Application.Users.GetUser;

public static class CreateUserHandler
{
    public static async Task<IResult> Handle(
        CreateUserValidator validator,
        CreateUserRepository repository,
        ILoggerFactory loggerFactory,
        CreateUserRequest request,
        CancellationToken ct = default)
    {
        var logger = loggerFactory.CreateLogger(nameof(CreateUserHandler));
        logger.LogInformation("Create new user {request}", request);

        // Input validation
        await validator.Validate(request, ct);

        try
        {
            // Todo: Map request to repository parameter object
            var userId = await repository.InsertUser(request.Username ?? "", ct);

            // Logging
            logger.LogInformation("User created. User-ID: {userId}", userId);

            var response = new CreateUserResponse { UserId = userId };

            return Results.CreatedAtRoute(
                GetUserHandler.RouteName,
                new { id = response.UserId },
                response);
        }
        catch (Exception exception)
        {
            // Log error
            logger.LogError(exception, "Failed to create user");

            throw;
        }
    }

}