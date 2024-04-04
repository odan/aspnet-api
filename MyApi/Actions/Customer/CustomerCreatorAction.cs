namespace MyApi.Actions.Customer;

using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Service;

public static class CustomerCreatorAction
{
    public static async Task<IResult> CreateUser(
        CustomerCreator userCreator,
        CustomerCreatorFormMapper mapper,
        [FromBody] CustomerCreatorFormData form
    )
    {
        var parameter = mapper.Map(form);

        var userId = await userCreator.CreateCustomer(parameter);

        return Results.Created("#", new { customer_id = userId });
    }

}