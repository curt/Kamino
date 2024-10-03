namespace Kamino.Shared.Models;

public class ProfileActivityModel : ActivityModelBase
{
    public Uri? Id { get; set; }

    public Uri? Url { get; set; }

    public Uri? Inbox { get; set; }

    public Uri? Outbox { get; set; }

    public Uri? Followers { get; set; }

    public Uri? Following { get; set; }

    public string? Icon { get; set; }

    public string? Name { get; set; }

    public string? PreferredUsername { get; set; }

    public object? PublicKey { get; set; }

    public string? Summary { get; set; }
}
