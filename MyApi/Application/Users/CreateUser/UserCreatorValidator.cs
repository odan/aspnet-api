namespace MyApi.Application.Users.CreateUser;

using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Controllers.Users.CreateUser;
using MyApi.Infrastruture;
using System.Reflection;
using System.Text.Json.Serialization;

public sealed class UserCreatorValidator : AbstractValidator<CreateUserRequest>
{
    private readonly UserCreatorRepository _repository;

    private readonly IStringLocalizer<UserCreatorValidator> _localizer;

    public UserCreatorValidator(
        UserCreatorRepository repository,
        IStringLocalizer<UserCreatorValidator> localizer
    )
    {
        _repository = repository;
        _localizer = localizer;

        // https://bit.ly/3jJ8Jcz
        RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(_localizer.GetString("Input required"))
            .MinimumLength(3).WithMessage(_localizer.GetString("Too short"))
            .MaximumLength(45).WithMessage(_localizer.GetString("Too long"))
            .NotEqual("admin").WithMessage(_localizer.GetString("Invalid value"))
            .Must(ValidateUsernameNotExists).WithMessage(_localizer.GetString("Username already taken"));

        RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(_localizer.GetString("Input required"))
            .Must(ValidateAge).WithMessage(_localizer.GetString("Invalid age"));
    }

    private bool ValidateUsernameNotExists(string username)
    {
        return !_repository.ExistsUsername(username);
    }

    private bool ValidateAge(DateTime? dateOfBirth)
    {
        return dateOfBirth.HasValue && Chronos.GetAge(dateOfBirth.Value) >= 18;
    }
}