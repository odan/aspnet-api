namespace Demo.Tests;

using Serilog.Sinks.InMemory;

public static class InMemorySinkExtensions
{
    public static void ShouldHaveMessage(
        this InMemorySink sink,
        string messageTemplate)
    {
        sink.LogEvents
            .Select(logEvent => logEvent.MessageTemplate.Text)
            .Should()
            .ContainSingle(text => text == messageTemplate);
    }
}