namespace Kamino.Shared.Models;

public class PongOutboundModel : ActivityModelBase
{
    public PongOutboundModel() { }

    public PongOutboundModel(Pong pong)
    {
        var ping = pong.Ping!;
        Id = pong.Uri;
        Type = "Pong";
        Actor = ping.ToUri;
        To = ping.ActorUri;
        Object = ping.Uri;
    }

    public Uri? Id { get; set; }

    public Uri? Actor { get; set; }

    public Uri? To { get; set; }

    public Uri? Object { get; set; }
}
