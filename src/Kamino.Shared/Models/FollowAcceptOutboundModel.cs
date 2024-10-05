namespace Kamino.Shared.Models;

public class FollowAcceptOutboundModel : ActivityModelBase
{
    public FollowAcceptOutboundModel(Follow follow)
    {
        Type = "Accept";
        Id = follow.AcceptUri;
        Actor = follow.ObjectUri;
        Object = new
        {
            Type = "Follow",
            Id = follow.Uri,
            Actor = follow.ActorUri,
            Object = follow.ObjectUri,
        };
    }

    public Uri? Id { get; set; }

    public Uri? Actor { get; set; }

    public object? Object { get; set; }
}
