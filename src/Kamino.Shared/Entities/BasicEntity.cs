namespace Kamino.Shared.Entities;

public abstract class BasicEntity : IdentifiableEntity, ITombstonable
{
    public Uri? Url { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? TombstonedAt { get; set; }
}
