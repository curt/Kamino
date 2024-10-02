namespace Kamino.Shared.Models;

public class PongOutboundModel : ActivityModelBase
{
    public PongOutboundModel() { }

    public PongOutboundModel(Pong pong)
    {
        var ping = pong.Ping!;
        Id = pong.ActivityUri;
        Type = "Pong";
        Actor = ping.ToUri;
        To = ping.ActorUri;
        Object = ping.ActivityUri;
    }

    public Uri? Id { get; set; }
    public Uri? Actor { get; set; }
    public Uri? To { get; set; }
    public Uri? Object { get; set; }
}
