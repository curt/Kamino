namespace Kamino.Shared.Models;

public class PostApiModelFactory(Uri endpoint) : ModelFactoryBase<Post, PostApiModel>(endpoint)
{
    public override PostApiModel Create(Post post)
    {
        return new PostApiModel()
        {
            Id = post.Id,
            Uri = UriInternalizer.Externalize(post.Uri),
            Url = UriInternalizer.Externalize(post.Url),
            PostType = post.PostType.ToString(),
            ContextUri = UriInternalizer.Externalize(post.ContextUri),
            InReplyToUri = UriInternalizer.Externalize(post.InReplyToUri),
            Slug = post.Slug,
            Title = post.Title,
            Summary = post.Summary,
            SourceType = post.SourceType.ToString(),
            Source = post.Source,
            StartsAt = post.StartsAt,
            EndsAt = post.EndsAt,
            PublishedAt = post.PublishedAt,
            EditedAt = post.EditedAt,
            AuthorUri = UriInternalizer.Externalize(post.Author?.Uri),
            Places = post.Places.Select(p => UriInternalizer.Externalize(p?.Uri) ?? string.Empty),
            Tags = post.Tags.Select(t => t?.Title ?? string.Empty),
        };
    }

    public override Post Parse(PostApiModel model)
    {
        return new Post()
        {
            Uri = UriInternalizer.Internalize(model.Uri),
            Url = UriInternalizer.Internalize(model.Url),
            PostType = Enum.TryParse(model.PostType, out PostType postType)
                ? postType
                : PostType.Note,
            ContextUri = UriInternalizer.Internalize(model.ContextUri),
            InReplyToUri = UriInternalizer.Internalize(model.InReplyToUri),
            Slug = model.Slug,
            Title = model.Title,
            Summary = model.Summary,
            SourceType = Enum.TryParse(model.SourceType, out SourceType sourceType)
                ? sourceType
                : SourceType.Markdown,
            Source = model.Source,
            StartsAt = model.StartsAt,
            EndsAt = model.EndsAt,
            PublishedAt = model.PublishedAt,
            EditedAt = model.EditedAt,
        };
    }
}
