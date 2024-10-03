using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class PostsApiService(Context context)
{
    public async Task<IEnumerable<PostApiModel>> GetPostsAsync()
    {
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
        Post post = CreatePost(postApiModel);
        post.Author = await SingleProfileAsync(postApiModel.AuthorUri!);
        context.Add(post);
        AfterAddPost(post);
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
        return await context
                .Profiles.WhereLocal()
                .Where(profile => profile.Uri == uri)
                .SingleOrDefaultAsync() ?? throw new BadRequestException();
    }

    private static void AfterAddPost(Post post)
    {
        if (post.Uri == null)
        {
            var guid = Medo.Uuid7.NewGuid();

            var uri = PostUriFromGuid(guid);
            post.Uri = uri;
            post.Url = uri;

            var contextUri = ContextUriFromGuid(guid);
            post.ContextUri ??= contextUri;
        }
    }

    private static Uri PostUriFromGuid(Guid guid) => UriFromGuid(guid, "/p/{0}");

    private static Uri ContextUriFromGuid(Guid guid) => UriFromGuid(guid, "/ctx/{0}");

    private static Uri UriFromGuid(Guid guid, string format)
    {
        var uri = new UriBuilder(Constants.LocalProfileUri)
        {
            Path = string.Format(format, guid.ToId22()),
        };

        return uri.Uri;
    }
}
