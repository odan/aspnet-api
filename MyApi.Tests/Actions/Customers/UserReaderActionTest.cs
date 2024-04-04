
namespace MyApi.Tests.Actions.Customers;

public class UserReaderActionTest
{
    [Fact]
    public void TestReadUser()
    {
        var app = new Application();
        app.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        app.InsertFixture("customers", new { username = "max", email = "max@example.com" });

        var response = app.CreateClient().GetAsync("/api/customers/1").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("max", result);
    }
}
