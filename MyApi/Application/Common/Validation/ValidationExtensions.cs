using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
    public static IList<ValidationResult> Validate([NotNull] this object model)
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

    /// <summary>
    /// Adds a validation error to the collection with the specified error message and member name.
    /// </summary>
    /// <remarks>
    /// <para>Example usage:</para>
    /// <code>
    /// errors.Add("Username already taken", nameof(command.Username));
    /// </code>
    /// </remarks>
    public static void Add([NotNull] this IList<ValidationResult> errors, string? errorMessage, string memberName)
    {
        errors.Add(new ValidationResult(errorMessage, [memberName]));
    }

    /// <summary>
    /// Throws validation exception if there are any validation errors.
    /// </summary>
    /// <param name="errors">Validation errors (can be null)</param>
    /// <param name="modelType">Type of the validated model (for better error messages)</param>
    /// <exception cref="InputValidationException">When results contain errors</exception>
    public static void ThrowIfAny(
        this IList<ValidationResult>? errors,
        object? model = null
    )
    {
        if (errors is null || errors.Count == 0)
            return;

        throw new InputValidationException(errors, model?.GetType());
    }

    public static void ThrowIfAny(this IList<ValidationResult>? errors, object model, string message)
    {
        if (errors is null || errors.Count == 0)
            return;

        throw new InputValidationException(errors, model?.GetType(), message);
    }

    public static void ThrowIfAny(this IList<ValidationResult>? errors, string message)
    {
        if (errors is null || errors.Count == 0)
            return;

        throw new InputValidationException(errors, message);
    }
}