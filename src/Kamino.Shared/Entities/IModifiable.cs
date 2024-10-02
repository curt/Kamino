namespace Kamino.Shared.Entities;

public interface IModifiable : ICreatable
{
    DateTime? ModifiedAt { get; set; }
}
