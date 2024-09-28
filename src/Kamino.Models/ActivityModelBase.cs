namespace Kamino.Models;

public abstract class ActivityModelBase
{
    [JsonPropertyName("@context")]
    public string? JsonLdContext { get; set; } = "https://www.w3.org/ns/activitystreams";
    public string? Type { get; set; }
}
