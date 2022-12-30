namespace MyApi.Domain.User.Service;

using FluentValidation;
using MyApi.Domain.User.Data;

public class UserCreatorFormValidator : AbstractValidator<UserCreatorForm>
{
    public UserCreatorFormValidator()
    {
        // Make formal validation (required, data types, codes,
        // length, ranges, data / date format, etc.) here.
        // The contextual validation (consistency,
        // uniqueness, complex rules, etc.) is then performed
        // in the application or domain service.

        this.RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Required")
            .NotEmpty().WithMessage("Required")
            .MaximumLength(45).WithMessage("Too long")
            .NotEqual("root").WithMessage("Invalid value");

        this.RuleFor(user => user.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Required")
            .NotEmpty().WithMessage("Required")
            .Matches(@"^[0-9]{4}\-[0-9]{2}\-[0-9]{2}$").WithMessage("Invalid date format")
            .Must(value => DateTime.TryParse(value, out _)).WithMessage("Invalid date");
    }
}
