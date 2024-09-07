using System;

namespace Kamino.Entities;

public abstract class IdentifiableEntity : ICreatable
{
    public Guid? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
}
