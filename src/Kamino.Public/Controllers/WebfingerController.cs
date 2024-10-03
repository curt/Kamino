using Kamino.Shared.Repo;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Public.Controllers;

[Route(".well-known/webfinger")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class WebfingerController(IDbContextFactory<NpgsqlContext> contextFactory) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string resource)
    {
        using var context = contextFactory.CreateDbContext();
        var service = new ProfilesService(context);
        var model = await service.GetPublicProfileByResourceAsync(resource);

        return Json(model);
    }
}
