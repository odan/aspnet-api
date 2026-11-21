namespace Demo.Api.Application.Users.CreateUser;

using Demo.Api.Application.Util.Validation;
using Demo.Api.Infrastructure.Clock;

public sealed class CreateUserValidator(CreateUserRepository repository)
{
    private readonly CreateUserRepository _repository = repository;

    public async Task Validate(CreateUserRequest command, CancellationToken ct = default)
    {
        // Quick validations first -> throw early -> detailed later
        // Do all fast/cheap checks first (DataAnnotations, simple rules), throw immediately if they fail.
        // Only run DB checks if the quick ones passed.

        // 1. Cheap & fast -> throw immediately (no DB hit)
        // DataAnnotations validation
        var errors = command.Validate().ThrowIfAny(command);

        // 2. Now safe to do business / DB validation
        var username = command.Username?.Trim();
        if (!string.IsNullOrWhiteSpace(username) &&
            await _repository.ExistsUsername(username, ct))
        {
            errors.AddError("Username already taken", nameof(command.Username));
        }

        if (command.DateOfBirth is DateTime dob && Chronos.GetAge(dob) < 18)
        {
            errors.AddError("Must be over 18 years old", nameof(command.DateOfBirth));
        }

        // Throws InputValidationException if there are any validation errors
        errors.ThrowIfAny(command);
    }

}