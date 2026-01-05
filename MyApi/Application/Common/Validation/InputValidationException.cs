using System.ComponentModel.DataAnnotations;

namespace MyApi.Application.Common.Validation;

public sealed class InputValidationException : Exception
{
    public IList<ValidationResult> Results { get; }

    public Type? ModelType { get; }

    public InputValidationException(
        IList<ValidationResult> results,
        string message = "One or more validation errors occurred."
    ) : base(message)
    {
        Results = results;
    }

    public InputValidationException(
        IList<ValidationResult> results,
        Type? modelType = null,
        string message = "One or more validation errors occurred."
) : base(message)
    {
        Results = results;
        ModelType = modelType;
    }
}