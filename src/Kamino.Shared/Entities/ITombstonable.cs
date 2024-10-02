namespace Kamino.Shared.Entities;

public interface ITombstonable : IModifiable
{
    DateTime? TombstonedAt { get; set; }
}
