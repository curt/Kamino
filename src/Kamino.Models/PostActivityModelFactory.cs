namespace Kamino.Models;

public class PostActivityModelFactory(Uri endpoint)
    : ModelFactoryBase<Post, PostActivityModel>(endpoint)
{
    public override PostActivityModel Create(Post post)
    {
        return new PostActivityModel()
        {
            Id = UriInternalizer.Externalize(post.Uri),
            Url = UriInternalizer.Externalize(post.Url),
            Type = post.PostType.ToString(),
            Context = UriInternalizer.Externalize(post.ContextUri),
            Conversation = UriInternalizer.Externalize(post.ContextUri),
            InReplyTo = UriInternalizer.Externalize(post.InReplyToUri),
            Name = post.Title,
            Summary = post.Summary,
            Content = post.Source,
            Published = post.PublishedAt,
            Updated = post.EditedAt,
            To = ["https://www.w3.org/ns/activitystreams#Public"],
            AttributedTo = UriInternalizer.Externalize(post.Author?.Uri),
        };
    }

    public override Post Parse(PostActivityModel model)
    {
        return new Post()
        {
            Uri = UriInternalizer.Internalize(model.Id),
            Url = UriInternalizer.Internalize(model.Url),
            PostType = Enum.TryParse(model.Type, out PostType postType) ? postType : PostType.Note,
            ContextUri = UriInternalizer.Internalize(model.Context),
            InReplyToUri = UriInternalizer.Internalize(model.InReplyTo),
            Title = model.Name,
            Summary = model.Summary,
            Source = model.Content,
            PublishedAt = model.Published,
            EditedAt = model.Updated,
        };
    }
}
