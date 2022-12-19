
namespace MyApi.Tests.Controllers.Users;

public class UserReaderControllerTest
{
    [Fact]
    public void TestFindUsers()
    {
        var app = new Application();
        app.ClearTables();

        var client = app.CreateClient();

        var content = app.CreateJson(new { username = "john" });
        var response = client.PostAsync("/api/users", content).Result;

        content = app.CreateJson(new { username = "sally" });
        response = client.PostAsync("/api/users", content).Result;

        response = client.GetAsync("/api/users").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("john", result);
        Assert.Contains("sally", result);
    }
}
