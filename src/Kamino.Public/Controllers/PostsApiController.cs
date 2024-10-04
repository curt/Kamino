using Kamino.Shared.Models;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kamino.Public.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsApiController(PostsApiService postsApiService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostApiModel>>> GetAll()
    {
        var posts = await postsApiService.GetPostsAsync();

        return posts.ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PostApiModel>> Post(PostApiModel model)
    {
        var post = await postsApiService.PostPostAsync(model);

        return CreatedAtAction(nameof(GetAll), new { id = post.Uri }, post);
    }
}
