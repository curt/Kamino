using Kamino.Entities;

namespace Kamino.Models;

public class PostApiModelFactory
{
    private UriInternalizer _internalizer;

    public PostApiModelFactory(Uri endpoint)
    {
        _internalizer = new UriInternalizer(endpoint);
    }

    public UriInternalizer UriInternalizer { get { return _internalizer; } }

    public PostApiModel Create(Post post)
    {
        return new PostApiModel()
        {
            Id = post.Id,
            Uri = _internalizer.Externalize(post.Uri),
            Url = _internalizer.Externalize(post.Url),
            PostType = post.PostType.ToString(),
            ContextUri = _internalizer.Externalize(post.ContextUri),
            InReplyToUri = _internalizer.Externalize(post.InReplyToUri),
            Slug = post.Slug,
            Title = post.Title,
            Summary = post.Summary,
            SourceType = post.SourceType.ToString(),
            Source = post.Source,
            StartsAt = post.StartsAt,
            EndsAt = post.EndsAt,
            PublishedAt = post.PublishedAt,
            EditedAt = post.EditedAt,
            AuthorUri = _internalizer.Externalize(post.Author?.Uri),
            Places = post.Places.Select(p => _internalizer.Externalize(p?.Uri) ?? string.Empty),
            Tags = post.Tags.Select(t => t?.Title ?? string.Empty)
        };
    }

    public Post Parse(PostApiModel model)
    {
        return new Post()
        {
            Uri = _internalizer.Internalize(model.Uri),
            Url = _internalizer.Internalize(model.Url),
            PostType = Enum.TryParse(model.PostType, out PostType postType) ? postType : PostType.Note,
            ContextUri = _internalizer.Internalize(model.ContextUri),
            InReplyToUri = _internalizer.Internalize(model.InReplyToUri),
            Slug = model.Slug,
            Title = model.Title,
            Summary = model.Summary,
            SourceType = Enum.TryParse(model.SourceType, out SourceType sourceType) ? sourceType : SourceType.Markdown,
            Source = model.Source,
            StartsAt = model.StartsAt,
            EndsAt = model.EndsAt,
            PublishedAt = model.PublishedAt,
            EditedAt = model.EditedAt,
        };
    }
}
