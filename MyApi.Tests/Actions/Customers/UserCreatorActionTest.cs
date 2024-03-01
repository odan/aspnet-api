
namespace MyApi.Tests.Actions.Customers;

public class UserCreatorActionTest
{
    [Fact]
    public void TestCreateCustomer()
    {
        var app = new Application();
        app.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 12, 31));

        var client = app.CreateClient();
        var content = app.CreateJson(new { username = "john", date_of_birth = "1982-03-28" });
        var response = client.PostAsync("/api/customers", content).Result;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(/*lang=json,strict*/ "{\"customer_id\":1}", response.Content.ReadAsStringAsync().Result);

        app.GetLoggerEvents()
            .Should()
            .HaveMessage("Customer created. Customer-ID: 1")
            .Appearing().Once();
    }

    [Fact]
    public void TestCreateUserValidation()
    {
        var app = new Application();
        app.ClearTables();

        var client = app.CreateClient();
        var content = app.CreateJson(new
        {
            username = "root",
            date_of_birth = "1982-03-28"
        });

        var response = client.PostAsync("/api/customers", content).Result;

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var json = response.Content.ReadAsStringAsync().Result;
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
