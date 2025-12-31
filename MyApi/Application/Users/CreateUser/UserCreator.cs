using MyApi.Shared.Extensions;
using MyApi.Shared.Support;

namespace MyApi.Application.Users.CreateUser;

public sealed class UserCreator(
    UserCreatorRepository repository,
    ITransaction transaction,
    ILoggerFactory factory
)
{
    private readonly UserCreatorRepository _repository = repository;

    private readonly ITransaction _transaction = transaction;

    private readonly ILogger<UserCreator> _logger = factory
            .WriteToFile("user_creator")
            .CreateLogger<UserCreator>();

    public async Task<int> CreateUser(UserCreateParameter user)
    {
        _logger.LogInformation("Create new user {user}", user);

        _transaction.Begin();

        try
        {
            var userId = await _repository.InsertUser(user.Username);

            _transaction.Commit();

            // Logging
            _logger.LogInformation($"User created. User-ID: {userId}");

            return userId;
        }
        catch (Exception exception)
        {
            _transaction.Rollback();

            _logger.LogError(exception.Message);

            throw;
        }
    }
}