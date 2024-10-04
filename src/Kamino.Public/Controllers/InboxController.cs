using System.Text.Json.Nodes;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("")]
public class InboxController(InboxService inboxService) : ControllerBase
{
    [HttpPost("inbox.json")]
    public async Task<IActionResult> Post([FromBody] JsonObject model)
    {
        await inboxService.ReceiveAsync(model);

        return Accepted();
    }
}
