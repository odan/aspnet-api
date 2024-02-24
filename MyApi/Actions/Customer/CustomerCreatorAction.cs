namespace MyApi.Actions.Customer;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Service;

public sealed class CustomerCreatorAction(
    CustomerCreator userCreator,
    CustomerCreatorFormMapper mapper
)
{
    private readonly CustomerCreator _userCreator = userCreator;
    private readonly CustomerCreatorFormMapper _mapper = mapper;

    public IResult CreateUser(CustomerCreatorFormData form)
    {
        var parameter = _mapper.Map(form);

        var userId = _userCreator.CreateCustomer(parameter);

        return Results.Created("#", new { customer_id = userId });
    }

}
