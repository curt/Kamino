namespace Kamino.Models;

public class ProfileActivityModel : ActivityModelBase
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Url { get; set; }
    public string? Inbox { get; set; }
    public string? Followers { get; set; }
    public string? Following { get; set; }
    public string? Icon { get; set; }
    public string? Name { get; set; }
    public string? PreferredUsername { get; set; }
    public object? PublicKey { get; set; }
    public string? Summary { get; set; }
}
