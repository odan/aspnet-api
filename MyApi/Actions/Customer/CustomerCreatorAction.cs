namespace MyApi.Actions.Customer;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Service;

public class CustomerCreatorAction
{
    private readonly CustomerCreator userCreator;
    private readonly CustomerCreatorFormMapper mapper;

    public CustomerCreatorAction(
        CustomerCreator userCreator,
        CustomerCreatorFormMapper mapper
    )
    {
        this.userCreator = userCreator;
        this.mapper = mapper;
    }

    public IResult CreateUser(CustomerCreatorFormData form)
    {
        var parameter = this.mapper.Map(form);

        var userId = this.userCreator.CreateUser(parameter);

        return Results.Created("-", new { customer_id = userId });
    }

}
