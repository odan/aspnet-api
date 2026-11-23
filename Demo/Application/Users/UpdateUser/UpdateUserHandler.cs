namespace Demo.Application.Users.UpdateUser;

public static class UpdateUserHandler
{
    public static async Task<IResult> Handle(
        UpdateUserValidator validator,
        UpdateUserRepository repository,
        ILoggerFactory loggerFactory,
        int id,
        UpdateUserRequest request,
        CancellationToken ct = default)
    {
        var logger = loggerFactory.CreateLogger(nameof(UpdateUserHandler));
        logger.LogInformation("Update user {id} {request}", id, request);

        await validator.Validate(id, request, ct);

        try
        {
            await repository.UpdateUser(id, request.Username ?? "", request.Email, ct);

            logger.LogInformation("User updated. User-ID: {userId}", id);

            return Results.NoContent();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to update user");

            throw;
        }
    }
}