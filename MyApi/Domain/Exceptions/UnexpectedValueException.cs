
namespace MyApi.Domain.Exceptions;

public class UnexpectedValueException : Exception
{
    public UnexpectedValueException(string message) : base(message) { }
}
