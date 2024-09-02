namespace Kamino.Entities;

public class Post : ICreatable, IModifiable, ITombstonable
{
    public Guid? Id { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Source { get; set; }
    public SourceType SourceType { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime? CachedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? TombstonedAt { get; set; }

    public virtual Profile? Author { get; set; }
}
