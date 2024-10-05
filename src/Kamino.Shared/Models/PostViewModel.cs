namespace Kamino.Shared.Models;

public class PostViewModel : ViewModelBase
{
    public Uri? Uri { get; set; }

    public Uri? Url { get; set; }

    public string? PostType { get; set; }

    public Uri? ContextUri { get; set; }

    public Uri? InReplyToUri { get; set; }

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public DateTime? StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public Uri? AuthorUri { get; set; }

    public Uri? AuthorUrl { get; set; }

    public string? AuthorName { get; set; }
}
