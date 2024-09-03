using Kamino.Entities;
using Kamino.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Services;

public class PostsService(Context context) : IPostsService
{
    public async Task<IEnumerable<Post>> GetPublicPostsAsync()
    {
        var now = DateTime.UtcNow;
        var posts = await context.Posts.WherePublished(now).WhereNotTombstoned().ToListAsync();

        return posts;
    }

    public async Task<Post> GetPublicPostByIdAsync(Guid id)
    {
        var now = DateTime.UtcNow;
        var posts = await context.Posts.WhereIdMatch(id).ToListAsync();

        return SinglePublicPost(posts, now);
    }

    private static Post SinglePublicPost(IEnumerable<Post> posts, DateTime before)
    {
        if (!posts.Any())
        {
            throw new NotFoundException();
        }

        var published = posts.AsQueryable().WherePublished(before);

        if (!published.Any())
        {
            throw new ForbiddenException();
        }

        var notTombstoned = published.WhereNotTombstoned();

        if (!notTombstoned.Any())
        {
            throw new GoneException();
        }

        return notTombstoned.Single();
    }
}
