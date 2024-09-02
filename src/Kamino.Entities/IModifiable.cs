namespace Kamino.Entities;

public interface IModifiable
{
    DateTime? ModifiedAt { get; set; }
}
