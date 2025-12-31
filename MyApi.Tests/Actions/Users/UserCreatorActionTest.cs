using MyApi.Shared.Support;

namespace MyApi.Tests.Actions.Users;

public class UserCreatorActionTest(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async void TestCreateUser()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 12, 31));

        var client = _factory.CreateClient();
        var content = _factory.CreateJson(new { username = "john", dateOfBirth = "1982-03-28" });
        var response = await client.PostAsync("/users", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("{\"userId\":1}", await response.Content.ReadAsStringAsync());

        _factory.GetLoggerEvents()
            .Should()
            .HaveMessage("User created. User-ID: 1")
            .Appearing().Once();
    }

    [Fact]
    public async void TestCreateUserValidation()
    {
        _database.ClearTables();

        var client = _factory.CreateClient();
        var content = _factory.CreateJson(new
        {
            username = "root",
            dateOfBirth = "1982-03-28"
        });

        var response = await client.PostAsync("/users", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Input validation failed", json);

        var expected = JsonSerializer.Serialize(new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title = "Input validation failed",
            status = 400,
            errors = new Dictionary<string, string[]>()
            {
                ["username"] = ["Invalid value"]
            }
        });

        Assert.Equal(expected, json);
    }
}