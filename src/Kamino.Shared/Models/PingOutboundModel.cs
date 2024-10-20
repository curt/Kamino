namespace Kamino.Shared.Models;

public class PingOutboundModel : ActivityModelBase
{
    public PingOutboundModel() { }

    public PingOutboundModel(Ping ping)
    {
        Id = ping.Uri;
        Type = "Ping";
        Actor = ping.ActorUri;
        To = ping.ToUri;
    }

    public Uri? Id { get; set; }

    public Uri? Actor { get; set; }

    public Uri? To { get; set; }
}
