using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Infrastructure;

namespace MyApi.Application.Users.CreateUser;

public sealed class UserCreator(
    UserCreatorValidator validator,
    IStringLocalizer<UserCreator> localizer,
    UserCreatorRepository repository,
    ITransaction transaction,
    ILogger<UserCreator> logger)
{
    private readonly UserCreatorValidator _validator = validator;
    private readonly IStringLocalizer<UserCreator> _localizer = localizer;
    private readonly UserCreatorRepository _repository = repository;
    private readonly ITransaction _transaction = transaction;
    private readonly ILogger<UserCreator> _logger = logger;

    public async Task<int> CreateUser(CreateUserCommand command, CancellationToken ct = default)
    {
        _logger.LogInformation("Create new user {request}", command);

        // Input validation
        await Validate(command, ct);

        _transaction.Begin();

        try
        {
            var userId = await _repository.InsertUser(command.Username, ct);

            _transaction.Commit();

            // Logging
            _logger.LogInformation("User created. User-ID: {userId}", userId);

            return userId;
        }
        catch (Exception exception)
        {
            _transaction.Rollback();

            _logger.LogError(exception, "Failed to create user");

            throw;
        }
    }

    private async Task Validate(CreateUserCommand command, CancellationToken ct)
    {
        var results = await _validator.ValidateAsync(command, ct);

        if (!results.IsValid)
        {
            throw new ValidationException(
                _localizer.GetString("Input validation failed"),
                results.Errors
            );
        }
    }
}