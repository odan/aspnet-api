namespace MyApi.Tests.Controllers.Users;

public class UserFinderControllerTest
{
    [Fact]
    public void Test()
    {
        var client = new Application().CreateClient();
        var response = client.GetAsync("/api/users").Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadAsStringAsync().Result;
        Assert.Contains("users", result);
    }
}
