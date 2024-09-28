namespace Kamino.Models;

public class PongOutboundModel : ActivityModelBase
{
    public PongOutboundModel() { }

    public PongOutboundModel(Pong pong)
    {
        var ping = pong.Ping!;
        Id = pong.ActivityUri;
        Actor = ping.ToUri;
        To = ping.ActorUri;
        Object = ping.ActivityUri;
    }

    public static string Type => "Pong";
    public Uri? Id { get; set; }
    public Uri? Actor { get; set; }
    public Uri? To { get; set; }
    public Uri? Object { get; set; }
}
