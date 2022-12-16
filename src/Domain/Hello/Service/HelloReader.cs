
namespace Domain.Hello.Service;

using Domain.Hello.Repository;

public class HelloReader
{
    private readonly HelloRepository repository;

    public HelloReader(HelloRepository repository) => this.repository = repository;

    public string ReadSomething()
    {
        var users = this.repository.FindUser();
        var userString = "";

        foreach (var user in users)
        {
            userString += ", " + user.Username;
        }

        return "Hello " + userString + " Time:  " +
            DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
    }
}

