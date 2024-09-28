using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("following")]
public class FollowingController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Empty;
    }
}
