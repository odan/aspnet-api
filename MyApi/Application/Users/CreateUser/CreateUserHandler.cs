using MyApi.Infrastructure.Persistence.Transactions;

namespace MyApi.Application.Users.CreateUser;

public sealed class CreateUserHandler(
    CreateUserValidator validator,
    //  IStringLocalizer<CreateUserHandler> localizer,
    CreateUserRepository repository,
    ITransaction transaction,
    ILogger<CreateUserHandler> logger)
{
    private readonly CreateUserValidator _validator = validator;
    //private readonly IStringLocalizer<CreateUserHandler> _localizer = localizer;
    private readonly CreateUserRepository _repository = repository;
    private readonly ITransaction _transaction = transaction;
    private readonly ILogger<CreateUserHandler> _logger = logger;

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        _logger.LogInformation("Create new user {request}", command);

        // Input validation
        await _validator.Validate(command, ct);

        _transaction.Begin();

        try
        {
            // Todo: Map command to repository parameter object
            var userId = await _repository.InsertUser(command.Username ?? "", ct);

            _transaction.Commit();

            // Logging
            _logger.LogInformation("User created. User-ID: {userId}", userId);

            return new CreateUserResult { UserId = userId };
        }
        catch (Exception exception)
        {
            // Rollback all changes on error
            _transaction.Rollback();

            // Log error
            _logger.LogError(exception, "Failed to create user");

            throw;
        }
    }
}