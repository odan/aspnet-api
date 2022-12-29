
namespace MyApi.Tests.Controllers.Users;

public class UserReaderControllerTest
{
    [Fact]
    public void TestFindUsers()
    {
        var app = new Application();
        app.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        var client = app.CreateClient();

        var content = app.CreateJson(new { username = "john", date_of_birth = "1982-03-28" });
        var response = client.PostAsync("/api/users", content).Result;

        content = app.CreateJson(new { username = "sally", date_of_birth = "2000-01-31" });
        response = client.PostAsync("/api/users", content).Result;

        response = client.GetAsync("/api/users").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("john", result);
        Assert.Contains("sally", result);
    }
}
