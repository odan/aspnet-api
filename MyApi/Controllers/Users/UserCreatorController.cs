namespace MyApi.Controllers.Users;

using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.User.Service;
using FluentValidation;

[ApiController]
public class UserCreatorController : Controller
{
    private readonly UserCreator userCreator;

    public UserCreatorController(UserCreator userCreator)
    {
        this.userCreator = userCreator;
    }

    [Route("api/users")]
    [HttpPost]
    public CreatedResult CreateUser([FromBody] JsonDocument data)
    {
        var user = this.Validate(data);

        //var username = data.RootElement.GetProperty("username").GetString();

        var userId = this.userCreator.CreateUser(user.username);

        return this.Created("", new { user_id = userId });
    }

    private UserCreatorRequest Validate(JsonDocument document)
    {
        var json = document.RootElement.ToString();
        var user = JsonSerializer.Deserialize<UserCreatorRequest>(json);

        if (user == null)
        {
            throw new ValidationException("Input required");
        }

        var validator = new UserCreatorRequestValidator();
        var results = validator.Validate(user);

        if (!results.IsValid)
        {
            throw new ValidationException("Input validation failed", results.Errors);
        }

        return user;
    }


    // https://learn.microsoft.com/en-us/answers/questions/1030059/how-to-validate-json-using-c-schema-validation.html
    public class UserCreatorRequest
    {
        public string username { get; set; } = "";
    }

    public class UserCreatorRequestValidator : AbstractValidator<UserCreatorRequest>
    {
        public UserCreatorRequestValidator()
        {
            RuleFor(user => user.username)
                .NotNull().WithMessage("Required")
                .NotEmpty().WithMessage("Required")
                .MaximumLength(45).WithMessage("Too long")
                .NotEqual("root").WithMessage("Invalid value");
        }
    }
}

