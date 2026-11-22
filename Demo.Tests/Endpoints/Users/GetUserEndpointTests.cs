using Demo.Api.Application.Users.GetUser;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Execution;
using System.Net.Http.Json;

namespace Demo.Tests.Endpoints.Users;

public class GetUserEndpointTests : IClassFixture<ApplicationFactory<Program>>
{
    private readonly ApplicationFactory<Program> _factory;

    public GetUserEndpointTests(ApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestReadUser()
    {
        _factory.Database.ClearTables();

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