namespace MyApi.Domain.User.Service;

using FluentValidation;
using MyApi.Domain.User.Data;

public class UserCreatorFormMapper
{
    private readonly UserCreatorFormValidator validator;

    public UserCreatorFormMapper(UserCreatorFormValidator validator)
    {
        this.validator = validator;
    }

    public UserCreatorParameter Map(UserCreatorForm? form)
    {
        // Input validation
        form = this.Validate(form);

        // Convert form data into a domain object
        return new UserCreatorParameter()
        {
            Username = form.Username,
            DateOfBirth = Chronos.ParseIsoDate(form.DateOfBirth),
        };
    }

    private UserCreatorForm Validate(UserCreatorForm? form)
    {
        if (form == null)
        {
            throw new ValidationException("Input required");
        }

        var results = this.validator.Validate(form);

        if (!results.IsValid)
        {
            throw new ValidationException("Input validation failed", results.Errors);
        }

        return form;
    }
}
