namespace MyApi.Domain.Customer.Repository;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Exceptions;

using SqlKata.Execution;

public sealed class CustomerRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public IEnumerable<Customer> FindCustomers()
    {
        return _db.Query("customers").Get<Customer>();
    }

    public Customer GetUserById(int id)
    {
        var user = _db.Query("customers").Where("id", id).FirstOrDefault<Customer>() ?? throw new DomainException("User not found");

        return user;
    }
}