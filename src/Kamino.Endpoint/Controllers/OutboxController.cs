using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("outbox")]
public class OutboxController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Empty;
    }
}
