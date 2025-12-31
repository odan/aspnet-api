
namespace MyApi.Application.Users.CreateUser;

using FluentValidation;

using Microsoft.Extensions.Localization;
using MyApi.Controllers.Users.CreateUser;
using MyApi.Shared.Support;

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
            .MaximumLength(45).WithMessage(_localizer.GetString("Too long"))
            .NotEqual("root").WithMessage(_localizer.GetString("Invalid value"))
            .Must(ValidateUsernameNotExists).WithMessage(_localizer.GetString("Username already exists"));
        ;

        RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^[0-9]{4}\-[0-9]{2}\-[0-9]{2}$").WithMessage(_localizer.GetString("Invalid date format"))
            .Must(value => DateTime.TryParse(value, out _)).WithMessage(_localizer.GetString("Invalid date"))
            .Must(ValidateAge).WithMessage(_localizer.GetString("Invalid age"));
    }

    private bool ValidateUsernameNotExists(string username)
    {
        return !_repository.ExistsUsername(username);
    }

    private bool ValidateAge(string dateOfBirth)
    {
        return Chronos.GetAge(Chronos.ParseIsoDate(dateOfBirth)) >= 18;
    }
}