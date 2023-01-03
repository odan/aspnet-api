
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerReader
{
    private readonly CustomerRepository _repository;

    public CustomerReader(CustomerRepository repository)
    {
        _repository = repository;
    }

    public Customer ReadUser(int id)
    {
        return _repository.GetUserById(id);
    }
}

