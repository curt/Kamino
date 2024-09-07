using Kamino.Endpoint.Models;
using Kamino.Services;
using Medo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint;

[Route("p")]
public class PostsController(IDbContextFactory<ApplicationContext> contextFactory) : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Index()
    {
        IEnumerable<PostViewModel> model;

        using (var context = contextFactory.CreateDbContext())
        {
            var postsService = new PostsService(context);
            var posts = await postsService.GetPublicPostsAsync();

            model = posts.Select(post => new PostViewModel(post, GetEndpoint()));
        }

        return View("index.html", model);
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Get(string id)
    {
        PostViewModel model;

        var guid = Uuid7.FromId22String(id).ToGuid();

        using (var context = contextFactory.CreateDbContext())
        {
            var postsService = new PostsService(context);
            var post = await postsService.GetPublicPostByIdAsync(guid);

            model = new PostViewModel(post, GetEndpoint());
        }

        return View("get.html", model);
    }

    private Uri GetEndpoint() => (new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1)).Uri;
}
