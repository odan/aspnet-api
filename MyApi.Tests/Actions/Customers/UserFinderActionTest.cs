namespace MyApi.Tests.Actions.Customers;

public class UserFinderActionTest
{
    private ApplicationFactory _factory { get; set; }
    private TestDatabase _database { get; set; }

    public UserFinderActionTest(
        ApplicationFactory factory,
        TestDatabase database
    )
    {
        _factory = factory;
        _database = database;
    }

    [Fact]
    public async void Test()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("customers", result);
    }

    [Fact]
    public async void TestFindUsers()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        var client = _factory.CreateClient();

        var content = _factory.CreateJson(new { username = "john", date_of_birth = "1982-03-28" });
        var response = await client.PostAsync("/api/customers", content);

        content = _factory.CreateJson(new { username = "sally", date_of_birth = "2000-01-31" });
        response = await client.PostAsync("/api/customers", content);

        response = await client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("john", result);
        Assert.Contains("sally", result);
    }
}