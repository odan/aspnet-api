using MyApi.Shared.Support;

namespace MyApi.Tests.Actions.Users;

public class UserFinderActionTest(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async void Test()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("users", result);
    }

    [Fact]
    public async void TestFindUsers()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        var client = _factory.CreateClient();

        var content = _factory.CreateJson(new { username = "john", dateOfBirth = "1982-03-28" });
        await client.PostAsync("/users", content);

        content = _factory.CreateJson(new { username = "sally", dateOfBirth = "2000-01-31" });
        await client.PostAsync("/users", content);

        var response = await client.GetAsync("/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("john", result);
        Assert.Contains("sally", result);
    }
}