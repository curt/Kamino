using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsApiController(IDbContextFactory<NpgsqlContext> contextFactory) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostApiModel>>> GetAll()
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsApiService(context);
        var posts = await postsService.GetPostsAsync();

        return posts.ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PostApiModel>> Post(PostApiModel model)
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsApiService(context);
        var post = await postsService.PostPostAsync(model);

        return CreatedAtAction(nameof(GetAll), new { id = post.Uri }, post);
    }
}
