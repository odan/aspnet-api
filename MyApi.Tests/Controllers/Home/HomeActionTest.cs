namespace MyApi.Tests.Controllers.Home;

public class HomeActionTest(ApplicationFactory factory)
{
    private readonly ApplicationFactory _factory = factory;

    [Fact]
    public async Task TestGet()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadAsStringAsync();
        actual.Should().Be("Hello, World!");

        _factory.LoggerEvents
            .Should()
            .HaveMessage("Home action")
            .Appearing().Once();
    }
}