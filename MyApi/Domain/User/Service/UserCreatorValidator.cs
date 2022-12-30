
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

        // Make formal validation (required, data types, codes,
        // length, ranges, data / date format, etc.) here.
        // The contextual validation (consistency,
        // uniqueness, complex rules, etc.) is then performed
        // in the application or domain service.

        this.RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Required")
            .MaximumLength(45).WithMessage("Too long")
            .NotEqual("root").WithMessage("Invalid value");

        this.RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .Must(this.ValidateAge).WithMessage("Invalid age");
    }

    private bool ValidateAge(DateTime dateOfBirth)
    {
        return Chronos.GetAge(dateOfBirth) >= 18;
    }
}
