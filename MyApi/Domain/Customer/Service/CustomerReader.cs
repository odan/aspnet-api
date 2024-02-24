
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerReader(CustomerRepository repository)
{
    private readonly CustomerRepository _repository = repository;

    public Customer ReadUser(int id)
    {
        return _repository.GetUserById(id);
    }
}

