using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.FindUser;
using MyApi.Infrastructure.Clock;
using System.Net.Http.Json;

namespace MyApi.Tests.Controllers.Users;

public class FinderUserControllerTest(
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

        var actual = await response.Content.ReadFromJsonAsync<FindUsersResult>();
        actual.Should().BeEquivalentTo(new FindUsersResult());
    }

    [Fact]
    public async void TestFindUsers()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 1, 1));

        var client = _factory.CreateClient();

        var content = new CreateUserCommand
        {
            Username = "john",
            Email = "john@example.com",
            Password = "securePassword123",
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1982, 03, 28),
        }.ToJsonContent();
        await client.PostAsync("/users", content);

        var content2 = new CreateUserCommand
        {
            Username = "sally",
            Email = "sally@example.com",
            Password = "securePassword123",
            FirstName = "Sally",
            LastName = "Doe",
            DateOfBirth = new DateTime(1980, 01, 31),
        }.ToJsonContent();
        await client.PostAsync("/users", content2);

        var response = await client.GetAsync("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<FindUsersResult>();

        actual.Should().BeEquivalentTo(new FindUsersResult
        {
            Users = [
                new() { Id = 1, Username = "john", Email = null },
                new() { Id = 2, Username = "sally", Email = null },
            ]
        });
    }
}