namespace MyApi.Application.Users.GetUser;

using FluentValidation;

using Microsoft.Extensions.Localization;
using MyApi.Application.Users.CreateUser;
using MyApi.Controllers.Users.CreateUser;
using MyApi.Shared.Support;

public sealed class UserRequestAdapter(
    UserCreatorValidator validator,
    IStringLocalizer<UserRequestAdapter> localizer
)
{
    private readonly UserCreatorValidator _validator = validator;
    private readonly IStringLocalizer<UserRequestAdapter> _localizer = localizer;

    public UserCreateParameter Map(CreateUserRequest request)
    {
        // Input validation
        request = Validate(request);

        // Convert form data into a domain object
        return new UserCreateParameter()
        {
            Username = request.Username,
            DateOfBirth = Chronos.ParseIsoDate(request.DateOfBirth),
        };
    }

    private CreateUserRequest Validate(CreateUserRequest form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var results = _validator.Validate(form);

        if (!results.IsValid)
        {
            throw new ValidationException(
                _localizer.GetString("Input validation failed"), results.Errors
            );
        }

        return form;
    }
}