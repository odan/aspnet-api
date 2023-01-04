
namespace MyApi.Tests.Actions.Home;

using Serilog.Sinks.InMemory.Assertions;
using Xunit.Abstractions;

public class HomeActionTest
{
    private readonly ITestOutputHelper _output;

    public HomeActionTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestGet()
    {
        var app = new Application();

        var client = app.CreateClient();
        var response = client.GetAsync("/").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, World!", response.Content.ReadAsStringAsync().Result);

        app.GetLoggerEvents()
            .Should()
            .HaveMessage("Home action")
            .Appearing().Once();
    }
}
