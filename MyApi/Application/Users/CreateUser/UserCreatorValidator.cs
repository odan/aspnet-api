namespace MyApi.Application.Users.CreateUser;

using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Infrastructure.Clock;

public sealed class UserCreatorValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UserCreatorRepository _repository;
    private readonly IStringLocalizer<UserCreatorValidator> _localizer;

    public UserCreatorValidator(
        UserCreatorRepository repository,
        IStringLocalizer<UserCreatorValidator> localizer)
    {
        _repository = repository;
        _localizer = localizer;

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(_localizer.GetString("Input required"))
            .MinimumLength(3).WithMessage(_localizer.GetString("Too short"))
            .MaximumLength(45).WithMessage(_localizer.GetString("Too long"))
            .NotEqual("admin").WithMessage(_localizer.GetString("Invalid value"))
            .MustAsync(ValidateUsernameNotExists)
            .OverridePropertyName("username")
            .WithMessage(_localizer.GetString("Username already taken"));

        RuleFor(user => user.DateOfBirth)
            .NotNull().WithMessage(_localizer.GetString("Input required"))
            .Must(ValidateAge)
            .OverridePropertyName("dateOfBirth")
            .WithMessage(_localizer.GetString("Invalid age"));
    }

    private async Task<bool> ValidateUsernameNotExists(
        string username,
        CancellationToken ct)
    {
        var exists = await _repository.ExistsUsername(username, ct);

        return !exists;
    }

    private static bool ValidateAge(DateTime? dateOfBirth)
        => dateOfBirth.HasValue && Chronos.GetAge(dateOfBirth.Value) >= 18;
}