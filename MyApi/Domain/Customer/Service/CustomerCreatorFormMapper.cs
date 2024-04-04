namespace MyApi.Domain.Customer.Service;

using FluentValidation;

using Microsoft.Extensions.Localization;

using MyApi.Domain.Customer.Data;

public sealed class CustomerCreatorFormMapper(
    CustomerCreatorValidator validator,
    IStringLocalizer<CustomerCreatorFormMapper> localizer
)
{
    private readonly CustomerCreatorValidator _validator = validator;
    private readonly IStringLocalizer<CustomerCreatorFormMapper> _localizer = localizer;

    public CustomerCreatorParameter Map(CustomerCreatorFormData? form)
    {
        // Input validation
        form = Validate(form);

        // Convert form data into a domain object
        return new CustomerCreatorParameter()
        {
            Username = form.Username,
            DateOfBirth = Chronos.ParseIsoDate(form.DateOfBirth),
        };
    }

    private CustomerCreatorFormData Validate(CustomerCreatorFormData? form)
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