
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public class CustomerCreator
{
    private readonly CustomerCreatorRepository repository;

    public CustomerCreator(CustomerCreatorRepository repository)
    {
        this.repository = repository;
    }

    public int CreateUser(CustomerCreatorParameter user)
    {
        var userId = this.repository.InsertUser(user.Username);

        // Logging
        // ...

        return userId;
    }
}

