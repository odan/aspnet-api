namespace Demo.Tests.Endpoints.Home;

public class HomeActionTest : IClassFixture<ApplicationFactory<Program>>
{
    private readonly ApplicationFactory<Program> _factory;

    public HomeActionTest(ApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestGet()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadAsStringAsync();
        actual.Should().Be("\"Hello, World!\"");

        _factory.Logger.ShouldHaveMessage("Home action");
    }
}