using Kamino.Entities;

namespace Kamino.Models;

public class PostViewModelFactory(Uri endpoint) : ModelFactoryBase<Post, PostViewModel>(endpoint)
{
    public override PostViewModel Create(Post post)
    {
        return new PostViewModel()
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
            Content = post.Source,
            StartsAt = post.StartsAt,
            EndsAt = post.EndsAt,
            PublishedAt = post.PublishedAt,
            EditedAt = post.EditedAt,
            AuthorUri = UriInternalizer.Externalize(post.Author?.Uri),
            AuthorUrl = UriInternalizer.Externalize(post.Author?.Url),
            AuthorName = post.Author?.DisplayName,
        };
    }

    public override Post Parse(PostViewModel model)
    {
        throw new NotImplementedException();
    }
}
