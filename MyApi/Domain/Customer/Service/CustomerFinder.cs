
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public class CustomerFinder
{
    private readonly CustomerRepository repository;

    public CustomerFinder(CustomerRepository repository)
    {
        this.repository = repository;
    }

    public IEnumerable<Customer> FindAllUsers()
    {
        var customers = this.repository.FindCustomers();

        // Custom logic...

        return customers;
    }
}

