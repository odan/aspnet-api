namespace Demo.Application.Users.DeleteUser;

public static class DeleteUserHandler
{
    public static async Task<IResult> Handle(
        DeleteUserRepository repository,
        ILoggerFactory loggerFactory,
        int id,
        CancellationToken ct = default)
    {
        var logger = loggerFactory.CreateLogger(nameof(DeleteUserHandler));
        logger.LogInformation("Delete user {id}", id);

        try
        {
            if (await repository.DeleteUser(id, ct) == 0)
            {
                return Results.NotFound();
            }

            logger.LogInformation("User deleted. User-ID: {userId}", id);

            return Results.NoContent();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to delete user");

            throw;
        }
    }
}