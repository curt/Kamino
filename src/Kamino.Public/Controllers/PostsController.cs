using Kamino.Shared.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[Route("posts")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class PostsController(PostsService postsService) : ContextualController
{
    [HttpGet("index.html")]
    public async Task<IActionResult> IndexHtml()
    {
        var postViewModels = await postsService.GetPostViewModelsAsync();

        return View("index.html", postViewModels);
    }

    [HttpGet("index.json")]
    public async Task<IActionResult> IndexJson()
    {
        var postActicityModels = await postsService.GetPostActivityModelsAsync();

        // TODO: Need to return ordered collection, not array.
        return Json(postActicityModels.Select(m => Contextify(m)));
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}.html")]
    public async Task<IActionResult> GetHtml(string id)
    {
        var postViewModel = await postsService.GetPostViewModelByUriAsync(
            new Uri(Request.GetEncodedUrl())
        );

        return View("get.html", postViewModel);
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}.json")]
    public async Task<IActionResult> GetJson(string id)
    {
        var postActivityModel = await postsService.GetPostActivityModelByUriAsync(
            new Uri(Request.GetEncodedUrl())
        );

        return Json(Contextify(postActivityModel));
    }
}
