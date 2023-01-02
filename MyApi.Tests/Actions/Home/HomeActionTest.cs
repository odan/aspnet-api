
namespace MyApi.Tests.Actions.Home;

public class HomeActionTest
{
    [Fact]
    public void TestGet()
    {
        var client = new Application().CreateClient();
        var response = client.GetAsync("/").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, World!", response.Content.ReadAsStringAsync().Result);
    }
}
