namespace Kamino.Shared.Entities;

public class Post : BasicEntity
{
    public PostType PostType { get; set; }
    public string? ContextUri { get; set; }
    public string? InReplyToUri { get; set; }
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public SourceType SourceType { get; set; }
    public string? Source { get; set; }
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime? CachedAt { get; set; }

    public virtual Profile? Author { get; set; }

    public virtual ICollection<Place> Places { get; } = [];

    public virtual ICollection<Tag> Tags { get; } = [];
}
