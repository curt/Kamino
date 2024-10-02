using System;

namespace Kamino.Shared.Entities;

public abstract class IdentifiableEntity : ICreatable
{
    public Guid? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
}
