using Kamino.Shared.Entities;
using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class PostsService(Context context)
{
    public async Task<IEnumerable<TModel>> GetPublicPostsAsync<TModel>(
        ModelFactoryBase<Post, TModel> factory
    )
    {
        var now = DateTime.UtcNow;
        var posts = await PublicPostsQueryBase()
            .WherePublished(now)
            .WhereNotTombstoned()
            .ToListAsync();

        return posts.Select(factory.Create);
    }

    public async Task<TModel> GetSinglePublicPostByIdAsync<TModel>(
        Guid id,
        ModelFactoryBase<Post, TModel> factory
    )
    {
        var now = DateTime.UtcNow;
        var posts = await PublicPostsQueryBase().WhereIdMatch(id).ToListAsync();

        var post = SinglePublicPost(posts, now);

        return factory.Create(post);
    }

    public Task<Post> CreateLocalPost(Post post)
    {
        throw new NotImplementedException();
    }

    private IQueryable<Post> PublicPostsQueryBase()
    {
        var profiles = context.Profiles.WhereLocal();
        var posts = context
            .Posts.Join(profiles, post => post.Author, profile => profile, (post, profile) => post)
            .Include(post => post.Author)
            .Include(post => post.Places)
            .Include(post => post.Tags);

        return posts;
    }

    private static Post SinglePublicPost(List<Post> posts, DateTime before)
    {
        if (posts.Count == 0)
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
