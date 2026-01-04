namespace MyApi.Infrastructure.Clock;

public interface IClock
{
    DateTime Now { get; }
}