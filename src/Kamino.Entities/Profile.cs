namespace Kamino.Entities;

public class Profile : ICreatable, IModifiable, ITombstonable
{
    public Guid? Id { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public string? Name { get; set; }
    public DateTime? CachedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? TombstonedAt { get; set; }

    public virtual ICollection<Post> Authored { get; } = [];
}
