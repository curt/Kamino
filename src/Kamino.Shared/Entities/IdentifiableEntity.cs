namespace Kamino.Shared.Entities;

public class IdentifiableEntity : ICreatable
{
    public Uri? Uri { get; set; }

    public DateTime? CreatedAt { get; set; }
}
