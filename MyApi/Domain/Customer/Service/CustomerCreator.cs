
namespace MyApi.Domain.Customer.Service;

using MyApi.Domain.Customer.Data;
using MyApi.Domain.Customer.Repository;
using Serilog;

public sealed class CustomerCreator
{
    private readonly CustomerCreatorRepository _repository;
    private readonly ILogger<CustomerCreator> _logger;

    public CustomerCreator(
        CustomerCreatorRepository repository,
        ILoggerFactory factory
    )
    {
        _repository = repository;
        _logger = factory.AddSerilog(
            new LoggerConfiguration()
            .WriteToFile("customer_creator")
            .CreateLogger()
        ).CreateLogger<CustomerCreator>();
    }

    public int CreateCustomer(CustomerCreatorParameter customer)
    {
        var customerId = _repository.InsertCustomer(customer.Username);

        // Logging
        _logger.LogInformation($"Customer created. Customer-ID: {customerId}");

        return customerId;
    }
}

