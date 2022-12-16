namespace Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
[ApiController]
public class IndexController : Controller
{
    [HttpGet()]
    public string Get() => "Hello, World!";

    [HttpPost]
    public IActionResult Post() => this.Ok("Hello, POST!");
}
