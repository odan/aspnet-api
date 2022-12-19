
namespace MyApi.Tests.Controllers.Users;

public class UserCreatorControllerTest
{
    [Fact]
    public void TestCreateUser()
    {
        var app = new Application();
        app.ClearTables();

        var client = app.CreateClient();
        var content = app.CreateJson(new { username = "john" });
        var response = client.PostAsync("/api/users", content).Result;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("{\"user_id\":1}", response.Content.ReadAsStringAsync().Result);
    }
}
