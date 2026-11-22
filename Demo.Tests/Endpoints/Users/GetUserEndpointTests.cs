using Demo.Api.Application.Users.GetUser;
using Demo.Api.Infrastructure.Clock;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Execution;
using System.Net.Http.Json;

namespace Demo.Tests.Endpoints.Users;

public class GetUserEndpointTests(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async Task TestReadUser()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        InsertUser("max", "max@example.com");

        var response = await _factory.CreateApiClient().GetAsync("/api/users/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<GetUserResponse>();
        actual.Should().BeEquivalentTo(new GetUserResponse
        {
            UserId = 1,
            UserName = "max",
        });
    }

    private void InsertUser(string username, string? email = null, DateTime? createdAt = null)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        db.Query("users").Insert(new
        {
            username,
            email,
            created_at = createdAt
        });
    }
}