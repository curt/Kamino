using Kamino.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("inbox")]
public class InboxController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] JsonElement model)
    {
        var service = new InboxService();
        service.Receive(model);

        return Accepted();
    }
}
