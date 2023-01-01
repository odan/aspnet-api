
namespace MyApi.Domain.User.Service;

using FluentValidation;
using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserCreatorValidator : AbstractValidator<UserCreatorFormData>
{
    private readonly UserCreatorRepository repository;

    public UserCreatorValidator(UserCreatorRepository repository)
    {
        this.repository = repository;

        // https://bit.ly/3jJ8Jcz
        this.RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Required")
            .MaximumLength(45).WithMessage("Too long")
            .NotEqual("root").WithMessage("Invalid value")
            .Must(this.ValidateUsernameNotExists).WithMessage("Username already exists");
        ;

        this.RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^[0-9]{4}\-[0-9]{2}\-[0-9]{2}$").WithMessage("Invalid date format")
            .Must(value => DateTime.TryParse(value, out _)).WithMessage("Invalid date")
            .Must(this.ValidateAge).WithMessage("Invalid age");
    }

    private bool ValidateUsernameNotExists(string username)
    {
        return !this.repository.ExistsUsername(username);
    }

    private bool ValidateAge(string dateOfBirth)
    {
        return Chronos.GetAge(Chronos.ParseIsoDate(dateOfBirth)) >= 18;
    }
}
