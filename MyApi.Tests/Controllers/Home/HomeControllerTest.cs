using System.Net;
using System.Text;
using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MyApi.Tests.Controllers.Home;

public class HomeControllerTest
{
    [Fact]
    public void TestGet()
    {
        var client = (new Application()).CreateClient();
        var response = client.GetAsync("/").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, World!", response.Content.ReadAsStringAsync().Result);
    }

    [Fact]
    public void TestPost()
    {
        var client = (new Application()).CreateClient();

        var data = new { key = "value" };
        string json = JsonSerializer.Serialize(data);
        StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = client.PostAsync("/", httpContent).Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello, POST!", response.Content.ReadAsStringAsync().Result);
    }
}
