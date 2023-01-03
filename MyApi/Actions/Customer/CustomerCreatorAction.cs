namespace MyApi.Actions.Customer;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Service;

public sealed class CustomerCreatorAction
{
    private readonly CustomerCreator _userCreator;
    private readonly CustomerCreatorFormMapper _mapper;

    public CustomerCreatorAction(
        CustomerCreator userCreator,
        CustomerCreatorFormMapper mapper
    )
    {
        _userCreator = userCreator;
        _mapper = mapper;
    }

    public IResult CreateUser(CustomerCreatorFormData form)
    {
        var parameter = _mapper.Map(form);

        var userId = _userCreator.CreateUser(parameter);

        return Results.Created("-", new { customer_id = userId });
    }

}
