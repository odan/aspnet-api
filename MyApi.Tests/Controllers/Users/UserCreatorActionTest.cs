using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers.Users.CreateUser;
using System.Net.Http.Json;

namespace MyApi.Tests.Controllers.Users;

public class UserCreatorActionTest(
    ApplicationFactory factory,
    TestDatabase database)
{
    private readonly ApplicationFactory _factory = factory;

    private readonly TestDatabase _database = database;

    [Fact]
    public async void TestCreateUser()
    {
        _database.ClearTables();

        Chronos.SetTestNow(new DateTime(2023, 12, 31));

        var client = _factory.CreateClient();

        var content = new CreateUserRequest
        {
            Username = "john",
            DateOfBirth = new DateTime(1982, 03, 28),
        }.ToJsonContent();
        var response = await client.PostAsync("/users", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

        result.Should().BeEquivalentTo(new CreateUserResponse
        {
            UserId = 1,
        });

        _factory.LoggerEvents
            .Should()
            .HaveMessage("User created. User-ID: {userId}")
            .Appearing().Once();
    }

    [Fact]
    public async void TestCreateUserValidation()
    {
        _database.ClearTables();

        var client = _factory.CreateClient();
        var content = new
        {
            username = "admin",
            dateOfBirth = "1982-03-28"
        }.ToJsonContent();

        var response = await client.PostAsync("/users", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        var expectedResponse = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Input validation failed",
            Status = 400,
            Errors =
            {
                ["username"] = ["Invalid value"]
            }
        };

        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
}