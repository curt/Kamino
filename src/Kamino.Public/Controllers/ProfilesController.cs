using Kamino.Shared.Models;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[Route("")]
[ResponseCache(Duration = 30)]
public class ProfilesController(ProfilesService profilesService) : ContextualController
{
    [HttpGet("")]
    public IActionResult IndexRedirect()
    {
        return RedirectPermanent(Url.Action(nameof(IndexHtml))!);
    }

    [HttpGet("index.html")]
    public async Task<IActionResult> IndexHtml()
    {
        return await Task.FromResult(View("index.html"));
    }

    [HttpGet("index.json")]
    public async Task<IActionResult> IndexJson()
    {
        var model = await profilesService.GetPublicProfileAsync();
        Response.ContentType = "application/activity+json; charset=utf-8";
        return Json(Contextify(model));
    }
}
