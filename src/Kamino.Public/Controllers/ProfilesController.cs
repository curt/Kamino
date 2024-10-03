using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Public.Controllers;

[Route("")]
[ResponseCache(Duration = 30)]
public class ProfilesController(IDbContextFactory<NpgsqlContext> contextFactory)
    : ContextualController
{
    [HttpGet("")]
    public IActionResult IndexRedirect()
    {
        return RedirectPermanent(Url.Action(nameof(IndexHtml))!);
    }

    [HttpGet("index.html")]
    [Produces("text/html")]
    public async Task<IActionResult> IndexHtml()
    {
        return await Task.FromResult(View("index.html"));
    }

    [HttpGet("index.json")]
    [Produces("application/json", "application/activity+json", "application/ld+json")]
    public async Task<IActionResult> IndexJson()
    {
        var model = await IndexModel();

        return Json(Contextify(model));
    }

    private async Task<ProfileActivityModel> IndexModel()
    {
        using var context = contextFactory.CreateDbContext();

        var service = new ProfilesService(context);
        var model = await service.GetPublicProfileAsync();

        return model;
    }
}
