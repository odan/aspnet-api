
namespace MyApi.Domain.Exceptions;

public sealed class UnexpectedValueException(string message) : Exception(message)
{
}
