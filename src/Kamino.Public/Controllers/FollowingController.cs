using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("")]
public class FollowingController : ControllerBase
{
    [HttpGet("following.json")]
    public ActionResult Get()
    {
        return Empty;
    }
}
