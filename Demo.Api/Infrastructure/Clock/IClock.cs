namespace Demo.Api.Infrastructure.Clock;

public interface IClock
{
    DateTime Now { get; }
}