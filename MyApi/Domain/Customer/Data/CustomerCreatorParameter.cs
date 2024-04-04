
namespace MyApi.Domain.Customer.Data;

public class CustomerCreatorParameter
{
    public string Username { get; set; } = "";

    public DateTime DateOfBirth { get; set; } = new DateTime(1970, 1, 1);
}