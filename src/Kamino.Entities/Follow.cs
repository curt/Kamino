namespace Kamino.Entities;

public class Follow : IdentifiableEntity
{
    public Uri? ActivityUri { get; set; }
    public Uri? AcceptUri { get; set; }
    public Uri? ActorUri { get; set; }
    public Uri? ObjectUri { get; set; }
}
