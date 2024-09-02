namespace Kamino.Entities;

public interface ITombstonable
{
    DateTime? TombstonedAt { get; set; }
}
