namespace Kamino.Shared.Models;

public class ProfileWebfingerModel
{
    public IEnumerable<string>? Aliases { get; set; }

    public IEnumerable<LinkWebfingerModel>? Links { get; set; }

    public string? Subject { get; set; }
}
