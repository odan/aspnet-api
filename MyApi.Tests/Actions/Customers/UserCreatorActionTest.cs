
namespace MyApi.Tests.Actions.Customers;

public class UserCreatorActionTest
{

    private ApplicationFactory _factory { get; set; }
    private TestDatabase _database { get; set; }

    public UserCreatorActionTest(
        ApplicationFactory factory,
        TestDatabase database
    )
    {
        _factory = factory;
        _database = database;
    }

    [Fact]
    public async void TestCreateCustomer()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 12, 31));

        var client = _factory.CreateClient();
        var content = _factory.CreateJson(new { username = "john", date_of_birth = "1982-03-28" });
        var response = await client.PostAsync("/api/customers", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("{\"customer_id\":1}", await response.Content.ReadAsStringAsync());

        _factory.GetLoggerEvents()
            .Should()
            .HaveMessage("Customer created. Customer-ID: 1")
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
            date_of_birth = "1982-03-28"
        });

        var response = await client.PostAsync("/api/customers", content);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Input validation failed", json);

        var expected = JsonSerializer.Serialize(new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title = "Input validation failed",
            status = 422,
            errors = new Dictionary<string, string[]>()
            {
                ["username"] = ["Invalid value"]
            }
        });

        Assert.Equal(expected, json);
    }
}