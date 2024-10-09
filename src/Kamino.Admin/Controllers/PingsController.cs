using Kamino.Admin.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Admin.Controllers;

[ApiController]
[Authorize]
[Route("api/pings")]
public class PingsController(IPingsService pingsService) : ControllerBase
{
    public async Task<IActionResult> Index()
    {
        var pings = await pingsService.GetPingsAsync();
        return Ok(pings);
    }
}
