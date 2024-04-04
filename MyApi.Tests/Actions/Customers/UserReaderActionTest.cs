
namespace MyApi.Tests.Actions.Customers;

public class UserReaderActionTest
{
    private ApplicationFactory _factory { get; set; }
    private TestDatabase _database { get; set; }

    public UserReaderActionTest(
        ApplicationFactory factory,
        TestDatabase database
    )
    {
        _factory = factory;
        _database = database;
    }

    [Fact]
    public async void TestReadUser()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        _database.InsertFixture("customers", new { username = "max", email = "max@example.com" });

        var response = await _factory.CreateClient().GetAsync("/api/customers/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("max", result);
    }
}