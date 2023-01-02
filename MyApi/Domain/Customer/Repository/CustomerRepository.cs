namespace MyApi.Domain.Customer.Repository;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Exceptions;
using SqlKata.Execution;

public class CustomerRepository
{
    private readonly QueryFactory db;

    public CustomerRepository(QueryFactory db)
    {
        this.db = db;
    }

    public IEnumerable<Customer> FindCustomers()
    {
        return this.db.Query("users").Get<Customer>();
    }

    public Customer GetUserById(int id)
    {
        var user = this.db.Query("users").Where("id", id).FirstOrDefault<Customer>();

        if (user == null)
        {
            throw new DomainException("User not found");
        }

        return user;
    }
}
