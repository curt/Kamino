using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("followers")]
public class FollowersController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Empty;
    }
}
