
namespace MyApi.Domain.Exceptions;

public sealed class UnexpectedValueException : Exception
{
    public UnexpectedValueException(string message) : base(message) { }
}
