using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route(".well-known/webfinger")]
[ResponseCache(Duration = 30)]
public class WebfingerController(ProfilesService profilesService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string resource)
    {
        var model = await profilesService.GetPublicProfileByResourceAsync(resource);

        return Json(model);
    }
}
