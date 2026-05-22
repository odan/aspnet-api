namespace Demo.Api.Infrastructure.Clock;

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}