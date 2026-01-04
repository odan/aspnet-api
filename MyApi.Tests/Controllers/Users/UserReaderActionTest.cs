using MyApi.Controllers.Users.GetUser;
using MyApi.Infrastructure.Clock;
using System.Net.Http.Json;

namespace MyApi.Tests.Controllers.Users;

public class UserReaderActionTest(
    ApplicationFactory factory,
    TestDatabase database)
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<GetUserResponse>();
        actual.Should().BeEquivalentTo(new GetUserResponse
        {
            UserId = 1,
            UserName = "max",
        });
    }
}