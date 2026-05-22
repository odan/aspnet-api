using System.ComponentModel.DataAnnotations;

namespace MyApi.Application.Common.Validation;

public static class ValidationExtensions
{
    /// <summary>
    /// Validates the object using DataAnnotations and returns validation results.
    /// Returns empty collection when model is valid.
    /// </summary>
    /// <param name="model">Object to validate</param>
    /// <returns>List of validation errors (empty when valid)</returns>
    /// <exception cref="ArgumentNullException">When model is null</exception>
    public static IList<ValidationResult> Validate(this object model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            results,
            validateAllProperties: true
        );

        return results;
    }

    /// <summary>
    /// Throws an exception if the model is not valid.
    /// </summary>
    public static void EnsureValid(this object model, string? message = null)
    {
        var errors = model.Validate();

        if (message is null)
        {
            errors.ThrowIfAny(model);
        }
        else
        {
            errors.ThrowIfAny(model, message);
        }
    }

    /// <summary>
    /// Throws validation exception if there are any validation errors.
    /// </summary>
    public static void ThrowIfAny(this IList<ValidationResult>? errors, object? model = null)
        => InputValidationException.ThrowIfAny(errors, model);

    public static void ThrowIfAny(this IList<ValidationResult>? errors, object? model, string message)
        => InputValidationException.ThrowIfAny(errors, model, message);

    public static void ThrowIfAny(this IList<ValidationResult>? errors, string message)
        => InputValidationException.ThrowIfAny(errors, null, message);

    /// <summary>
    /// Adds a validation error to the collection with the specified error message and member name.
    /// </summary>
    /// <remarks>
    /// <para>Example usage:</para>
    /// <code>
    /// errors.Add("Username already taken", nameof(command.Username));
    /// </code>
    /// </remarks>
    public static void AddError(this IList<ValidationResult> errors, string? errorMessage, string memberName)
    {
        ArgumentNullException.ThrowIfNull(errors);
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);

        errors.Add(new ValidationResult(errorMessage, [memberName]));
    }
}