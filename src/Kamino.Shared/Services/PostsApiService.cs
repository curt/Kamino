using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Medo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class PostsApiService(
    IDbContextFactory<NpgsqlContext> contextFactory,
    IdentifierProvider identifierProvider
)
{
    public async Task<IEnumerable<PostApiModel>> GetPostsAsync()
    {
        using var context = contextFactory.CreateDbContext();
        var posts = await context
            .Posts.Include(post => post.Author)
            .Include(post => post.Places)
            .Include(post => post.Tags)
            .ToListAsync();

        var models = posts.Select(CreatePostApiModel);

        return models;
    }

    public async Task<PostApiModel> PostPostAsync(PostApiModel postApiModel)
    {
        using var context = contextFactory.CreateDbContext();
        Post post = CreatePost(postApiModel);
        AfterCreatePost(post);
        post.Author = await SingleProfileAsync(postApiModel.AuthorUri!);
        context.Add(post);
        context.SaveChanges();

        return CreatePostApiModel(post);
    }

    private static PostApiModel CreatePostApiModel(Post post) =>
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
            SourceType = post.SourceType.ToString(),
            Source = post.Source,
            StartsAt = post.StartsAt,
            EndsAt = post.EndsAt,
            PublishedAt = post.PublishedAt,
            EditedAt = post.EditedAt,
            AuthorUri = post.Author?.Uri,
            Places = post.Places.Select(p => p.Uri!),
            Tags = post.Tags.Select(t => t.NormalizedTitle!),
        };

    private static Post CreatePost(PostApiModel postApiModel) =>
        new()
        {
            Uri = postApiModel.Uri,
            Url = postApiModel.Url,
            PostType = Enum.TryParse(postApiModel.PostType, out PostType postType)
                ? postType
                : PostType.Note,
            ContextUri = postApiModel.ContextUri,
            InReplyToUri = postApiModel.InReplyToUri,
            Slug = postApiModel.Slug,
            Title = postApiModel.Title,
            Summary = postApiModel.Summary,
            SourceType = Enum.TryParse(postApiModel.SourceType, out SourceType sourceType)
                ? sourceType
                : SourceType.Markdown,
            Source = postApiModel.Source,
            StartsAt = postApiModel.StartsAt,
            EndsAt = postApiModel.EndsAt,
            PublishedAt = postApiModel.PublishedAt,
            EditedAt = postApiModel.EditedAt,
        };

    private async Task<Profile> SingleProfileAsync(Uri uri)
    {
        using var context = contextFactory.CreateDbContext();
        return await context
                .Profiles.WhereUriMatch(identifierProvider.GetProfileJson())
                .Where(profile => profile.Uri == uri)
                .SingleOrDefaultAsync() ?? throw new BadRequestException();
    }

    private void AfterCreatePost(Post post)
    {
        if (post.Uri == null)
        {
            var uuid7 = Uuid7.NewUuid7();
            post.Uri = identifierProvider.GetPostJson(uuid7);
            post.Url = identifierProvider.GetPostHtml(uuid7);
            post.ContextUri ??= identifierProvider.GetTag(uuid7, "conversation");
        }
    }
}
