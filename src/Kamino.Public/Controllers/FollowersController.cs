using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("")]
public class FollowersController : ControllerBase
{
    [HttpGet("followers.json")]
    public ActionResult Get()
    {
        return Empty;
    }
}
