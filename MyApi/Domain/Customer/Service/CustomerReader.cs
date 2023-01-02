
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public class CustomerReader
{
    private readonly CustomerRepository repository;

    public CustomerReader(CustomerRepository repository)
    {
        this.repository = repository;
    }

    public Customer ReadUser(int id)
    {
        return this.repository.GetUserById(id);
    }
}

