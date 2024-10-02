namespace Kamino.Shared.Entities;

public class Tag : IdentifiableEntity
{
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? NormalizedTitle { get; set; }
    public string? Summary { get; set; }
    public SourceType SourceType { get; set; }
    public string? Source { get; set; }

    public virtual ICollection<Post> Posts { get; } = [];
    public virtual ICollection<Place> Places { get; } = [];
}
