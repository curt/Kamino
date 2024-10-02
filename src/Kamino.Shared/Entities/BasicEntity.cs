namespace Kamino.Shared.Entities;

public abstract class BasicEntity : IdentifiableEntity, ITombstonable
{
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? TombstonedAt { get; set; }
}
