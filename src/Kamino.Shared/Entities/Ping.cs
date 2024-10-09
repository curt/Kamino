namespace Kamino.Shared.Entities;

public class Ping : IdentifiableEntity
{
    public Uri? ActorUri { get; set; }

    public virtual Profile? Actor { get; set; }

    public Uri? ToUri { get; set; }

    public virtual Profile? To { get; set; }

    public virtual ICollection<Pong> Pongs { get; set; } = [];
}
