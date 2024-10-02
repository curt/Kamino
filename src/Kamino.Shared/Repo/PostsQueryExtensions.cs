using Kamino.Shared.Entities;

namespace Kamino.Shared.Repo;

public static class PostsQueryExtensions
{
    public static IQueryable<Post> WhereIdMatch(this IQueryable<Post> posts, Guid id)
    {
        return posts.Where(post => post.Id == id);
    }

    public static IQueryable<Post> WhereUriMatch(this IQueryable<Post> posts, string uri)
    {
        return posts.Where(post => post.Uri == uri);
    }

    public static IQueryable<Post> WherePublished(this IQueryable<Post> posts, DateTime before)
    {
        return posts.Where(post => post.PublishedAt != null && post.PublishedAt <= before);
    }

    public static IQueryable<Post> WhereNotTombstoned(this IQueryable<Post> posts)
    {
        return posts.Where(post => post.TombstonedAt == null);
    }
}
