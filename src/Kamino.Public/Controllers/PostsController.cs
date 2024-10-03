using Kamino.Shared.Repo;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Public.Controllers;

[Route("posts")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class PostsController(IDbContextFactory<NpgsqlContext> contextFactory) : ContextualController
{
    [HttpGet("index.html")]
    public async Task<IActionResult> IndexHtml()
    {
        using var context = contextFactory.CreateDbContext();
        var postViewModels = await new PostsService(context).GetPostViewModelsAsync();

        return View("index.html", postViewModels);
    }

    [HttpGet("index.json")]
    public async Task<IActionResult> IndexJson()
    {
        using var context = contextFactory.CreateDbContext();
        var postActicityModels = await new PostsService(context).GetPostActivityModelsAsync();

        // TODO: Need to return ordered collection, not array.
        return Json(postActicityModels.Select(m => Contextify(m)));
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}.html")]
    public async Task<IActionResult> GetHtml(string id)
    {
        using var context = contextFactory.CreateDbContext();
        var postViewModel = await new PostsService(context).GetPostViewModelByUriAsync(
            new Uri(Request.GetEncodedUrl())
        );

        return View("get.html", postViewModel);
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}.json")]
    public async Task<IActionResult> GetJson(string id)
    {
        using var context = contextFactory.CreateDbContext();
        var postActivityModel = await new PostsService(context).GetPostActivityModelByUriAsync(
            new Uri(Request.GetEncodedUrl())
        );

        return Json(Contextify(postActivityModel));
    }
}
