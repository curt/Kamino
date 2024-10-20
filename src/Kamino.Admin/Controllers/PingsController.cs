using Kamino.Admin.Client.Models;
using Kamino.Admin.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Admin.Controllers;

[ApiController]
[Authorize]
[Route("api/pings")]
public class PingsController(IPingsService pingsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var pings = await pingsService.GetPingsAsync();
        return Ok(pings);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PingApiModel ping)
    {
        var result = await pingsService.SendPingAsync(ping.ToUri!);
        return Ok(result);
    }
}
