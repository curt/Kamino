namespace Kamino.Shared.Models;

public class PostActivityModel : ActivityModelBase
{
    public Uri? Id { get; set; }

    public Uri? Url { get; set; }

    public Uri? AttributedTo { get; set; }

    public Uri? InReplyTo { get; set; }

    public Uri? Context { get; set; }

    public Uri? Conversation { get; set; }

    public string? Name { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public DateTime? Published { get; set; }

    public DateTime? Updated { get; set; }

    public IEnumerable<Uri> To { get; set; } = [];

    public IEnumerable<Uri> Cc { get; set; } = [];
}
