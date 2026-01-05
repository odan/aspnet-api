namespace MyApi.Application.Users.CreateUser;

using MyApi.Application.Common.Validation;
using MyApi.Infrastructure.Clock;
using System.ComponentModel.DataAnnotations;

public sealed class CreateUserValidator(CreateUserRepository repository)
{
    private readonly CreateUserRepository _repository = repository;

    public async Task Validate(CreateUserCommand command, CancellationToken ct)
    {
        var results = command.Validate();

        // fail fast: avoids database call
        results.ThrowIfInvalid(command);

        var username = command.Username?.Trim();
        if (!string.IsNullOrWhiteSpace(username) &&
            await _repository.ExistsUsername(username, ct))
        {
            results.Add(new ValidationResult("Username already taken", [nameof(command.Username)]));
        }

        if (command.DateOfBirth is DateTime dob && Chronos.GetAge(dob) < 18)
        {
            results.Add(new ValidationResult("Must be over 18 years old", [nameof(command.DateOfBirth)]));
        }

        results.ThrowIfInvalid(command);
    }

}