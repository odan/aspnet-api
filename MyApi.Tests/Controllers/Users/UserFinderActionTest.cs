using MyApi.Controllers.Users.SearchUsers;
using MyApi.Shared.Support;
using System.Net.Http.Json;

namespace MyApi.Tests.Controllers.Users;

public class UserFinderActionTest(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async void TestEmptyResult()
    {
        _database.ClearTables();

        var client = _factory.CreateClient();

        var response = await client.GetAsync("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<SearchUsersResponse>();
        actual.Should().BeEquivalentTo(new SearchUsersResponse());
    }

    [Fact]
    public async void TestFindUsers()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        var client = _factory.CreateClient();

        var content = new { username = "john", dateOfBirth = "1982-03-28" }.ToJsonContent();
        await client.PostAsync("/users", content);

        var content2 = new { username = "sally", dateOfBirth = "2000-01-31" }.ToJsonContent();
        await client.PostAsync("/users", content2);

        var response = await client.GetAsync("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<SearchUsersResponse>();

        actual.Should().BeEquivalentTo(new SearchUsersResponse
        {
            Users = [
                new() { Id = 1, Username = "john", Email = null },
                new() { Id = 2, Username = "sally", Email = null }
                ]
        });
    }
}