using Microsoft.Extensions.Localization;
using MyApi.Application.Common.Validation;
using MyApi.Infrastructure.Database;
using System.ComponentModel.DataAnnotations;

namespace MyApi.Application.Users.CreateUser;

public sealed class CreateUserCommandHandler(
    CreateUserValidator validator,
    IStringLocalizer<CreateUserCommandHandler> localizer,
    CreateUserRepository repository,
    ITransaction transaction,
    ILogger<CreateUserCommandHandler> logger)
{
    private readonly CreateUserValidator _validator = validator;
    private readonly IStringLocalizer<CreateUserCommandHandler> _localizer = localizer;
    private readonly CreateUserRepository _repository = repository;
    private readonly ITransaction _transaction = transaction;
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        _logger.LogInformation("Create new user {request}", command);

        // Input validation
        await _validator.Validate(command, ct);

        _transaction.Begin();

        try
        {
            var userId = await _repository.InsertUser(command.Username ?? "", ct);

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
}