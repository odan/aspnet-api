using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Controllers.Users.CreateUser;
using MyApi.Infrastruture;
using MyApi.Shared.Extensions;

namespace MyApi.Application.Users.CreateUser;

public sealed class UserCreator(
    UserCreatorValidator validator,
    IStringLocalizer<UserCreator> localizer,
    UserCreatorRepository repository,
    ITransaction transaction,
    ILoggerFactory factory
)
{
    private readonly UserCreatorValidator _validator = validator;

    private readonly IStringLocalizer<UserCreator> _localizer = localizer;

    private readonly UserCreatorRepository _repository = repository;

    private readonly ITransaction _transaction = transaction;

    private readonly ILogger<UserCreator> _logger = factory.CreateLogger<UserCreator>();

    public async Task<int> CreateUser(CreateUserRequest request)
    {
        _logger.LogInformation("Create new user {request}", request);

        // Input validation
        Validate(request);

        _transaction.Begin();

        try
        {
            var userId = await _repository.InsertUser(request.Username);

            _transaction.Commit();

            // Logging
            _logger.LogInformation("User created. User-ID: {userId}", userId);

            return userId;
        }
        catch (Exception exception)
        {
            _transaction.Rollback();

            _logger.LogError(exception.Message);

            throw;
        }
    }

    private void Validate(CreateUserRequest request)
    {
        var results = _validator.Validate(request);

        if (!results.IsValid)
        {
            throw new ValidationException(
                _localizer.GetString("Input validation failed"), results.Errors
            );
        }
    }
}