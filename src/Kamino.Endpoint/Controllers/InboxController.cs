using System.Text.Json.Nodes;
using Kamino.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("inbox")]
public class InboxController(IInboxService inboxService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JsonObject model)
    {
        await inboxService.ReceiveAsync(model);

        return Accepted();
    }
}
