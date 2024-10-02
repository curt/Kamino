using Kamino.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[Route("Error")]
public class ErrorController : ContextualController
{
    [HttpGet("{statusCode}")]
    public IActionResult Get(int statusCode)
    {
        var model = new StatusCodeViewModel()
        {
            Title = statusCode.ToString(),
            StatusCode = statusCode,
        };

        return View("get.html", model);
    }
}
