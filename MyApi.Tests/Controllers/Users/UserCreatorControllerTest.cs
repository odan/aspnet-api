
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

    [Fact]
    public void TestCreateUserValidation()
    {
        var app = new Application();
        app.ClearTables();

        var client = app.CreateClient();
        var content = app.CreateJson(new { username = "root" });
        var response = client.PostAsync("/api/users", content).Result;

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var json = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("Input validation failed", json);

        var expected = JsonSerializer.Serialize(new
        {
            error = new
            {
                message = "Input validation failed",
                details = new List<object> {
                    new
                    {
                        message = "Invalid value",
                        field = "username"
                    }
                }
            }
        });

        Assert.Equal(expected, json);
    }
}
