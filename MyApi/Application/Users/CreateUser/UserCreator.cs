using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Application.Users.GetUser;
using MyApi.Controllers.Users.CreateUser;
using MyApi.Shared.Extensions;
using MyApi.Shared.Support;
using Org.BouncyCastle.Asn1.Ocsp;

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

    private readonly ILogger<UserCreator> _logger = factory
            .WriteToFile("user_creator")
            .CreateLogger<UserCreator>();

    public async Task<int> CreateUser(CreateUserRequest request)
    {
        _logger.LogInformation("Create new user {request}", request);

        // Input validation
        var parameters = Validate(request);

        _transaction.Begin();

        try
        {
            var userId = await _repository.InsertUser(parameters.Username);

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

    private UserCreateParameter Validate(CreateUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var results = _validator.Validate(request);

        if (!results.IsValid)
        {
            throw new ValidationException(
                _localizer.GetString("Input validation failed"), results.Errors
            );
        }

        // Convert form data into a domain object
        return new UserCreateParameter()
        {
            Username = request.Username,
            DateOfBirth = Chronos.ParseIsoDate(request.DateOfBirth),
        };
    }
}