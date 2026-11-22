using System.ComponentModel.DataAnnotations;

namespace Demo.Application.Util.Validation;

public sealed class InputValidationException : Exception
{
    public IList<ValidationResult> Results { get; }

    public Type? ModelType { get; }

    public InputValidationException(
        IList<ValidationResult> results,
        Type? modelType = null,
        string message = "One or more validation errors occurred."
    ) : base(message)
    {
        ArgumentNullException.ThrowIfNull(results);
        Results = results;
        ModelType = modelType;
    }

    public static IList<ValidationResult> ThrowIfAny(
        IList<ValidationResult>? errors,
        object? model = null,
        string message = "One or more validation errors occurred."
    )
    {
        if (errors is null || errors.Count == 0)
            return errors ?? Array.Empty<ValidationResult>();

        throw new InputValidationException(errors, model?.GetType(), message);
    }
}