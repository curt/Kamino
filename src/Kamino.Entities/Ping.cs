namespace Kamino.Entities;

public class Ping : IdentifiableEntity
{
    public Uri? ActivityUri { get; set; }
    public Uri? ActorUri { get; set; }
    public Uri? ToUri { get; set; }
    public virtual ICollection<Pong> Pongs { get; } = [];
}
