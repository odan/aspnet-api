
namespace MyApi.Tests.Actions.Home;

public class HomeActionTest
{
    private ApplicationFactory _factory { get; set; }

    public HomeActionTest(ApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void TestGet()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, World!", await response.Content.ReadAsStringAsync());

        _factory.GetLoggerEvents()
            .Should()
            .HaveMessage("Home action")
            .Appearing().Once();
    }
}