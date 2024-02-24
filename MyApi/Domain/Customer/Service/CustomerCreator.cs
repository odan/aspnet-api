
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;

public sealed class CustomerCreator(
    CustomerCreatorRepository repository,
    ITransaction transaction,
    ILoggerFactory factory
)
{
    private readonly CustomerCreatorRepository _repository = repository;
    private readonly ITransaction _transaction = transaction;
    private readonly ILogger<CustomerCreator> _logger = factory
            .WriteToFile("customer_creator")
            .CreateLogger<CustomerCreator>();

    public int CreateCustomer(CustomerCreatorParameter customer)
    {
        _logger.LogInformation("Create new customer {Customer}", customer);

        _transaction.Begin();

        try
        {
            var customerId = _repository.InsertCustomer(customer.Username);

            _transaction.Commit();

            // Logging
            _logger.LogInformation($"Customer created. Customer-ID: {customerId}");

            return customerId;
        }
        catch (Exception exception)
        {
            _transaction.Rollback();

            _logger.LogError(exception.Message);

            throw;
        }
    }
}

