namespace Kamino.Entities;

public interface ITombstonable : IModifiable
{
    DateTime? TombstonedAt { get; set; }
}
