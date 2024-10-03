namespace Kamino.Shared.Entities;

public class Like : IdentifiableEntity
{
    public Uri? ActorUri { get; set; }

    public Uri? ObjectUri { get; set; }
}
