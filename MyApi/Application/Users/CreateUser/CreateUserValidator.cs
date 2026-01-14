namespace MyApi.Application.Users.CreateUser;

using MyApi.Application.Common.Validation;
using MyApi.Infrastructure.Clock;

public sealed class CreateUserValidator(CreateUserRepository repository)
{
    private readonly CreateUserRepository _repository = repository;

    public async Task Validate(CreateUserCommand command, CancellationToken ct)
    {
        // Cheap validations first -> throw early -> expensive later
        // Do all fast/cheap checks first (DataAnnotations, simple rules), throw immediately if they fail.
        // Only run DB checks if the cheap ones passed.

        // 1. Cheap & fast -> throw immediately (no DB hit)
        // DataAnnotations validation
        var errors = command.Validate();

        // Throws InputValidationException if there are any validation errors
        errors.ThrowIfAny(command);

        // 2. Now safe to do business / DB validation
        var username = command.Username?.Trim();
        if (!string.IsNullOrWhiteSpace(username) &&
            await _repository.ExistsUsername(username, ct))
        {
            errors.Add("Username already taken", nameof(command.Username));
        }

        if (command.DateOfBirth is DateTime dob && Chronos.GetAge(dob) < 18)
        {
            errors.Add("Must be over 18 years old", nameof(command.DateOfBirth));
        }

        // Throws InputValidationException if there are any validation errors
        errors.ThrowIfAny(command);
    }

}