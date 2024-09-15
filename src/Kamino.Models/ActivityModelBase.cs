namespace Kamino.Models;

public abstract class ActivityModelBase
{
    [JsonPropertyName("@context")]
    public string? JsonLdContext { get; set; }
}
