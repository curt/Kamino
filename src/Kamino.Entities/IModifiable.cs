namespace Kamino.Entities;

public interface IModifiable : ICreatable
{
    DateTime? ModifiedAt { get; set; }
}
