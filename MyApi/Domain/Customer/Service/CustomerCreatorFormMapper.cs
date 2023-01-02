namespace MyApi.Domain.Customer.Service;

using FluentValidation;
using Microsoft.Extensions.Localization;
using MyApi.Domain.Customer.Data;

public class CustomerCreatorFormMapper
{
    private readonly CustomerCreatorValidator validator;
    private readonly IStringLocalizer<CustomerCreatorFormMapper> _localizer;

    public CustomerCreatorFormMapper(
        CustomerCreatorValidator validator,
        IStringLocalizer<CustomerCreatorFormMapper> localizer
    )
    {
        this.validator = validator;
        this._localizer = localizer;
    }

    public CustomerCreatorParameter Map(CustomerCreatorFormData? form)
    {
        // Input validation
        form = this.Validate(form);

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

        var results = this.validator.Validate(form);

        if (!results.IsValid)
        {
            throw new ValidationException(
                _localizer.GetString("Input validation failed"), results.Errors
            );
        }

        return form;
    }
}
