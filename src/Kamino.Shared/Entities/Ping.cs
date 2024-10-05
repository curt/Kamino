namespace Kamino.Shared.Entities;

public class Ping : IdentifiableEntity
{
    public Uri? ActorUri { get; set; }

    public Uri? ToUri { get; set; }

    public virtual ICollection<Pong> Pongs { get; } = [];
}
