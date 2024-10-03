namespace Kamino.Shared.Entities;

public class Follow : IdentifiableEntity
{
    public Uri? AcceptUri { get; set; }

    public Uri? ActorUri { get; set; }

    public Uri? ObjectUri { get; set; }
}
