using Kamino.Models;
using Kamino.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsApiController(IDbContextFactory<ApplicationContext> contextFactory) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostApiModel>>> GetAll()
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsApiService(context, Request.GetEndpoint());
        var posts = await postsService.GetPostsAsync();

        return posts.ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PostApiModel>> Post(PostApiModel model)
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsApiService(context, Request.GetEndpoint());
        var post = await postsService.PostPostAsync(model);

        return CreatedAtAction(nameof(GetAll), new { id = post.Uri }, post);
    }
}
