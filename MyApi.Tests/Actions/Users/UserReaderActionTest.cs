using MyApi.Shared.Support;

namespace MyApi.Tests.Actions.Users;

public class UserReaderActionTest(
    ApplicationFactory factory,
    TestDatabase database
    )
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async void TestReadUser()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        _database.InsertFixture("users", new { username = "max", email = "max@example.com" });

        var response = await _factory.CreateClient().GetAsync("/users/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("max", result);
    }
}