
namespace MyApi.Domain.User.Service;

using FluentValidation;
using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserCreatorValidator : AbstractValidator<UserCreatorParameter>
{
    private readonly UserCreatorRepository repository;

    public UserCreatorValidator(UserCreatorRepository repository)
    {
        this.repository = repository;

        this.RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Required")
            .MaximumLength(45).WithMessage("Too long")
            .NotEqual("root").WithMessage("Invalid value")
            .Must(this.ValidateUsernameNotExists).WithMessage("Username already exists");
        ;

        this.RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .Must(this.ValidateAge).WithMessage("Invalid age");
    }

    private bool ValidateUsernameNotExists(string username)
    {
        return !this.repository.ExistsUsername(username);
    }

    private bool ValidateAge(DateTime dateOfBirth)
    {
        return Chronos.GetAge(dateOfBirth) >= 18;
    }
}
