using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Users.CreateUser;
using MyApi.Infrastructure.Clock;
using System.Net.Http.Json;

namespace MyApi.Tests.Controllers.Users;

public class CreateUserControllerTests(
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

        var content = new CreateUserCommand
        {
            Username = "john",
            Email = "john@example.com",
            Password = "securePassword123",
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1982, 03, 28),
        }.ToJsonContent();
        var response = await client.PostAsync("/users", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResult>();

        result.Should().BeEquivalentTo(new CreateUserResult
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
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = 400,
            Instance = "/users",
            Errors =
            {
                ["email"] = ["E-Mail is required."],
                ["password"] = ["Password is required."],
                ["firstName"] = ["First name is required."],
                ["lastName"] = ["Last name is required."]
            }
        };


        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
}