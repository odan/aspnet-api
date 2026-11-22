using Demo.Api.Application.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Demo.Tests.Endpoints.Users;

public class CreateUserEndpointTests : IClassFixture<ApplicationFactory<Program>>
{
    private readonly ApplicationFactory<Program> _factory;

    public CreateUserEndpointTests(ApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestCreateUser()
    {
        _factory.Database.ClearTables();

        _factory.Time.SetUtcNow(new DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero));

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
        var response = await client.PostAsync("/api/users", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

        result.Should().BeEquivalentTo(new CreateUserResponse
        {
            UserId = 1,
        });

        _factory.Logger.ShouldHaveMessage("User created. User-ID: {userId}");
    }

    [Fact]
    public async Task TestCreateUserValidation()
    {
        _factory.Database.ClearTables();

        var client = _factory.CreateApiClient();
        var content = new
        {
            username = "admin",
            dateOfBirth = "1982-03-28"
        }.ToJsonContent();

        var response = await client.PostAsync("/api/users", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        var expectedResponse = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = 400,
            Instance = "/api/users",
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