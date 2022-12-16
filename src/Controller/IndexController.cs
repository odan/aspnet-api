using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("/")]
[ApiController]
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
        return Ok("Hello, POST!");
    }
}
