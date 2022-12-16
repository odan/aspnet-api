namespace MyApi.Controllers;
using Domain.Hello.Service;
using Microsoft.AspNetCore.Mvc;

[Route("api/hello")]
[ApiController]
public class HelloController : Controller
{
    private readonly HelloReader helloReader;

    public HelloController(HelloReader helloService) => this.helloReader = helloService;

    [HttpGet("{id}")]
    // api/hello/1234567
    public Product Get(int id)
    {
        var product = new Product
        {
            Id = id,
            Name = this.helloReader.ReadSomething()
        };

        return product;
    }

    [HttpPost]
    public IActionResult Post() => this.Ok("Hello, Post to api/hello");
}

public class Product
{
    // In order for the JsonSerializer to be able to
    // see and serialize your properties, they need to be public:
    public int Id { get; set; }
    public string? Name { get; set; }
}
