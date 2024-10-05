namespace Kamino.Shared.Models;

public class PostApiModel
{
    public Uri? Uri { get; set; }

    public Uri? Url { get; set; }

    public string? PostType { get; set; }

    public Uri? ContextUri { get; set; }

    public Uri? InReplyToUri { get; set; }

    public string? Slug { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public string? SourceType { get; set; }

    public string? Source { get; set; }

    public DateTime? StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public Uri? AuthorUri { get; set; }

    public IEnumerable<Uri> Places { get; set; } = [];

    public IEnumerable<string> Tags { get; set; } = [];
}
