namespace MyApi.Tests.Actions.Home;

public class HomeActionTest(ApplicationFactory factory)
{
    private readonly ApplicationFactory _factory = factory;

    [Fact]
    public async void TestGet()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, World!", await response.Content.ReadAsStringAsync());

        _factory.LoggerEvents
            .Should()
            .HaveMessage("Home action")
            .Appearing().Once();
    }
}