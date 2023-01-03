namespace MyApi.Domain.Customer.Service;

using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Domain.Customer.Data;

public sealed class CustomerCreatorFormMapper
{
    private readonly CustomerCreatorValidator _validator;
    private readonly IStringLocalizer<CustomerCreatorFormMapper> _localizer;

    public CustomerCreatorFormMapper(
        CustomerCreatorValidator validator,
        IStringLocalizer<CustomerCreatorFormMapper> localizer
    )
    {
        _validator = validator;
        _localizer = localizer;
    }

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
        if (form == null)
        {
            throw new ValidationException(_localizer.GetString("Input required"));
        }

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
