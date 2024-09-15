using Kamino.Models;
using Kamino.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("inbox")]
public class InboxController(IInboxService inboxService) : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] ObjectInboxModel model)
    {
        inboxService.Receive(model);

        return Accepted();
    }
}
