namespace MyApi.Shared.Exceptions;

public sealed class DomainException(string message) : Exception(message)
{
}