using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("")]
public class OutboxController : ControllerBase
{
    [HttpGet("outbox.json")]
    public ActionResult Get()
    {
        return Empty;
    }
}
