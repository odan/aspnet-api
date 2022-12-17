namespace MyApi.Controllers.Home;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class IndexController : Controller
{
    [HttpGet()]
    public string Get()
    {
        return "Hello, World!";
    }

    [HttpPost]
    public IActionResult Post()
    {
        return this.Ok("Hello, POST!");
    }
}
