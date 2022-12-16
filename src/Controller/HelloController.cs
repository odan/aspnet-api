using Microsoft.AspNetCore.Mvc;
using Domain.Hello.Service;

namespace MyApi.Controllers;

[Route("api/hello")]
[ApiController]
public class HelloController : Controller
{
    private readonly HelloReader _helloReader;

    public HelloController(HelloReader helloService)
    {
        _helloReader = helloService;
    }

    [HttpGet("{id}")]
    // api/hello/1234567
    public Product Get(int id)
    {
        var product = new Product();
        product.Id = id;
        product.Name = _helloReader.readSomething();

        return product;
    }

    [HttpPost]
    public IActionResult Post()
    {
        return Ok("Hello, Post to api/hello");
    }
}

public class Product
{
    // In order for the JsonSerializer to be able to 
    // see and serialize your properties, they need to be public:
    public int Id { get; set; }
    public string? Name { get; set; }
}