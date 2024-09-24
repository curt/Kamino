namespace Kamino.Models;

public class NormalizedActorModel
{
    public Uri? Id { get; set; }
    public string? Type { get; set; }
    public Uri? Url { get; set; }
    public Uri? Inbox { get; set; }
    public IEnumerable<Uri>? Followers { get; set; }
    public IEnumerable<Uri>? Following { get; set; }
    public string? Icon { get; set; }
    public string? Name { get; set; }
    public string? PreferredUsername { get; set; }
    public object? PublicKey { get; set; }
    public string? Summary { get; set; }
}
