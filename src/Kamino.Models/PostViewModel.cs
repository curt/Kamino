namespace Kamino.Models;

public class PostViewModel : ViewModelBase
{
    public Guid? Id { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public string? PostType { get; set; }
    public string? ContextUri { get; set; }
    public string? InReplyToUri { get; set; }
    public string? Slug { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorUri { get; set; }
    public string? AuthorUrl { get; set; }
    public string? AuthorName { get; set; }
}
