using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[Route(".well-known/webfinger")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class WebfingerController(ProfilesService profilesService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string resource)
    {
        var model = await profilesService.GetPublicProfileByResourceAsync(resource);

        return Json(model);
    }
}
