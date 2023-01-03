
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerFinder
{
    private readonly CustomerRepository _repository;

    public CustomerFinder(CustomerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Customer> FindAllUsers()
    {
        var customers = _repository.FindCustomers();

        // Custom logic...

        return customers;
    }
}

