namespace MyApi.Tests.Actions.Customers;

public class UserFinderActionTest
{
    [Fact]
    public void Test()
    {
        var client = new Application().CreateClient();
        var response = client.GetAsync("/api/customers").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("customers", result);
    }
}
