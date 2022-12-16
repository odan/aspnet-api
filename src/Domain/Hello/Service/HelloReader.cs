
namespace Domain.Hello.Service;

using Domain.Hello.Repository;

public class HelloReader
{
    private readonly HelloRepository _repository;

    public HelloReader(HelloRepository repository)
    {
        _repository = repository;
    }

    public string readSomething()
    {
        var users = _repository.findUser();
        var userString = "";

        foreach (var user in users)
        {
            userString += ", " + user.Username;
        }

        return "Hello " + userString + " Time:  " +
            DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");
    }
}

