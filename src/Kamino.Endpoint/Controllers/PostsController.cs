using Kamino.Services;
using Medo;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint
{
    [Route("p")]
    public class PostsController(IPostsService postsService) : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var posts = await postsService.GetPublicPostsAsync();

            return View(posts);
        }

        [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Get(string id)
        {
            var guid = Uuid7.FromId22String(id).ToGuid();
            var post = await postsService.GetPublicPostByIdAsync(guid);

            return View(post);
        }
    }
}
