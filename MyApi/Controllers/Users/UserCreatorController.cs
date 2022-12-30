namespace MyApi.Controllers.Users;

using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.User.Data;
using MyApi.Domain.User.Service;

[ApiController]
public class UserCreatorController : Controller
{
    private readonly UserCreator userCreator;
    private readonly UserCreatorFormValidator validator;

    public UserCreatorController(
        UserCreator userCreator,
        UserCreatorFormValidator validator
    )
    {
        this.userCreator = userCreator;
        this.validator = validator;
    }

    [Route("api/users")]
    [HttpPost]
    public CreatedResult CreateUser([FromBody] JsonDocument document)
    {
        // https://learn.microsoft.com/en-us/answers/questions/1030059/how-to-validate-json-using-c-schema-validation.html
        // Deserialize JSON payload to object
        var form = JsonSerializer.Deserialize<UserCreatorForm>(
            document.RootElement.ToString()
        );

        // Input validation
        form = this.Validate(form);

        // convert json or form data into a domain object
        var parameter = new UserCreatorParameter()
        {
            Username = form.Username,
            DateOfBirth = Chronos.ParseIsoDate(form.DateOfBirth),
        };

        var userId = this.userCreator.CreateUser(parameter);

        return this.Created("", new { user_id = userId });
    }

    private UserCreatorForm Validate(UserCreatorForm? form)
    {
        if (form == null)
        {
            throw new ValidationException("Input required");
        }

        var results = this.validator.Validate(form);


        //var errors = new List<FluentValidation.Results.ValidationFailure>();
        //errors.Add(new FluentValidation.Results.ValidationFailure("field", "message"));
        //throw new ValidationException("Input validation failed", errors);

        if (!results.IsValid)
        {
            throw new ValidationException("Input validation failed", results.Errors);
        }

        return form;
    }
}
