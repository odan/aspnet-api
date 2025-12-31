namespace MyApi.Shared.Exceptions;

public sealed class UnexpectedValueException(string message) : Exception(message)
{
}