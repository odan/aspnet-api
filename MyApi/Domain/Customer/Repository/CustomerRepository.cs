namespace MyApi.Domain.Customer.Repository;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Exceptions;
using SqlKata.Execution;

public sealed class CustomerRepository
{
    private readonly QueryFactory _db;

    public CustomerRepository(QueryFactory db)
    {
        _db = db;
    }

    public IEnumerable<Customer> FindCustomers()
    {
        return _db.Query("users").Get<Customer>();
    }

    public Customer GetUserById(int id)
    {
        var user = _db.Query("users").Where("id", id).FirstOrDefault<Customer>();

        if (user == null)
        {
            throw new DomainException("User not found");
        }

        return user;
    }
}
