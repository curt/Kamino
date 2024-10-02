using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

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
