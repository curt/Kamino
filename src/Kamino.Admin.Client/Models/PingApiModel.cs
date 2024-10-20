namespace Kamino.Admin.Client.Models;

public class PingApiModel
{
    public string? PingUri { get; set; }

    public DateTime? PingCreatedAt { get; set; }

    public string? PongUri { get; set; }

    public DateTime? PongCreatedAt { get; set; }

    public string? ActorUri { get; set; }

    public string? ActorDisplayName { get; set; }

    public string? ActorIcon { get; set; }

    public string? ToUri { get; set; }

    public string? ToDisplayName { get; set; }

    public string? ToIcon { get; set; }
}
