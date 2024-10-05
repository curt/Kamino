using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class PostsService(
    IDbContextFactory<NpgsqlContext> contextFactory,
    IdentifierProvider identifierProvider
)
{
    public async Task<IEnumerable<PostViewModel>> GetPostViewModelsAsync()
    {
        var now = DateTime.UtcNow;

        using var context = contextFactory.CreateDbContext();
        var posts = await PublicPostsQueryBase(context)
            .WherePublished(now)
            .WhereNotTombstoned()
            .ToListAsync();

        return posts.Select(CreatePostViewModel);
    }

    public async Task<IEnumerable<PostActivityModel>> GetPostActivityModelsAsync()
    {
        var now = DateTime.UtcNow;

        using var context = contextFactory.CreateDbContext();
        var posts = await PublicPostsQueryBase(context)
            .WherePublished(now)
            .WhereNotTombstoned()
            .ToListAsync();

        return posts.Select(CreatePostActivityModel);
    }

    public async Task<PostViewModel> GetPostViewModelByUriAsync(Uri uri)
    {
        var now = DateTime.UtcNow;

        using var context = contextFactory.CreateDbContext();
        var posts = await PublicPostsQueryBase(context).WhereUriMatch(uri).ToListAsync();
        var post = SinglePublicPost(posts, now);

        return CreatePostViewModel(post);
    }

    public async Task<PostActivityModel> GetPostActivityModelByUriAsync(Uri uri)
    {
        var now = DateTime.UtcNow;

        using var context = contextFactory.CreateDbContext();
        var posts = await PublicPostsQueryBase(context).WhereUriMatch(uri).ToListAsync();
        var post = SinglePublicPost(posts, now);

        return CreatePostActivityModel(post);
    }

    private static PostViewModel CreatePostViewModel(Post post) =>
        new()
        {
            Uri = post.Uri,
            Url = post.Url,
            PostType = post.PostType.ToString(),
            ContextUri = post.ContextUri,
            InReplyToUri = post.InReplyToUri,
            Slug = post.Slug,
            Title = post.Title,
            Summary = post.Summary,
            Content = post.Source,
            StartsAt = post.StartsAt,
            EndsAt = post.EndsAt,
            PublishedAt = post.PublishedAt,
            EditedAt = post.EditedAt,
            AuthorUri = post.Author?.Uri,
            AuthorUrl = post.Author?.Url,
            AuthorName = post.Author?.DisplayName,
        };

    private PostActivityModel CreatePostActivityModel(Post post) =>
        new()
        {
            Id = post.Uri,
            Url = post.Url,
            Type = post.PostType.ToString(),
            Context = post.ContextUri,
            Conversation = post.ContextUri,
            InReplyTo = post.InReplyToUri,
            Name = post.Title,
            Summary = post.Summary,
            Content = post.Source,
            Published = post.PublishedAt,
            Updated = post.EditedAt,
            To = [new Uri("https://www.w3.org/ns/activitystreams#Public")],
            Cc = [new UriBuilder(identifierProvider.GetBase()) { Path = "/followers" }.Uri],
            AttributedTo = post.Author?.Uri,
        };

    private IQueryable<Post> PublicPostsQueryBase(Context context)
    {
        var profiles = context.Profiles.WhereUriMatch(identifierProvider.GetProfileJson());
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
            throw new NotFoundException();

        var published = posts.AsQueryable().WherePublished(before);
        if (!published.Any())
            throw new ForbiddenException();

        var notTombstoned = published.WhereNotTombstoned();
        if (!notTombstoned.Any())
            throw new GoneException();

        return notTombstoned.Single();
    }
}
