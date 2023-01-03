
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerCreator
{
    private readonly CustomerCreatorRepository _repository;

    public CustomerCreator(CustomerCreatorRepository repository)
    {
        _repository = repository;
    }

    public int CreateUser(CustomerCreatorParameter user)
    {
        var userId = _repository.InsertUser(user.Username);

        // Logging
        // ...

        return userId;
    }
}

