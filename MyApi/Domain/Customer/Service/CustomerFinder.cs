
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerFinder(CustomerRepository repository)
{
    private readonly CustomerRepository _repository = repository;

    public IEnumerable<Customer> FindAllUsers()
    {
        var customers = _repository.FindCustomers();

        // Custom logic...

        return customers;
    }
}

