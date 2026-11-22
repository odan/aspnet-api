using Demo.Api.Application.Users.CreateUser;
using Demo.Api.Application.Users.FindUser;
using System.Net.Http.Json;

namespace Demo.Tests.Endpoints.Users;

public class FindUsersEndpointTests(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async Task TestEmptyResult()
    {
        _database.ClearTables();

        var client = _factory.CreateApiClient();

        var response = await client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<FindUsersResponse>();
        actual.Should().BeEquivalentTo(new FindUsersResponse());
    }

    [Fact]
    public async Task TestFindUsers()
    {
        _database.ClearTables();

        _factory.Time.SetUtcNow(new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero));

        var client = _factory.CreateApiClient();

        var content = new CreateUserRequest
        {
            Username = "john",
            Email = "john@example.com",
            Password = "securePassword123",
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1982, 03, 28),
        }.ToJsonContent();
        await client.PostAsync("/api/users", content);

        var content2 = new CreateUserRequest
        {
            Username = "sally",
            Email = "sally@example.com",
            Password = "securePassword123",
            FirstName = "Sally",
            LastName = "Doe",
            DateOfBirth = new DateTime(1980, 01, 31),
        }.ToJsonContent();
        await client.PostAsync("/api/users", content2);

        var response = await client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<FindUsersResponse>();

        actual.Should().BeEquivalentTo(new FindUsersResponse
        {
            Users = [
                new() { Id = 1, Username = "john", Email = null },
                new() { Id = 2, Username = "sally", Email = null },
            ]
        });
    }

    [Fact]
    public async Task TestApiKeyRequired()
    {
        var response = await _factory.CreateClient().GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}