namespace Kamino.Shared.Models;

public class PostApiModel
{
    public Guid? Id { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public string? PostType { get; set; }
    public string? ContextUri { get; set; }
    public string? InReplyToUri { get; set; }
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? SourceType { get; set; }
    public string? Source { get; set; }
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public string? AuthorUri { get; set; }
    public IEnumerable<string> Places { get; set; } = [];
    public IEnumerable<string> Tags { get; set; } = [];
}
