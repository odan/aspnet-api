using System.ComponentModel.DataAnnotations;

namespace MyApi.Application.Common.Validation;

public static class ValidationExtensions
{
    public static IList<ValidationResult> Validate(this object model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            model,
            context,
            results,
            validateAllProperties: true
        );

        return results;
    }

    public static void ValidateOrThrow(this object model)
    {
        var results = Validate(model);

        if (results.Count == 0)
            return;

        throw new InputValidationException(
            results,
            model.GetType()
        );
    }

    public static void ThrowIfInvalid(this IList<ValidationResult> results, object model)
    {
        if (results.Count == 0)
            return;

        throw new InputValidationException(results, model.GetType());
    }
}