namespace Kamino.Entities;

public class Like : IdentifiableEntity
{
    public Uri? ActivityUri { get; set; }
    public Uri? ActorUri { get; set; }
    public Uri? ObjectUri { get; set; }
}
