namespace Kamino.Models;

public class PostActivityModel : ActivityModelBase
{
    public string? Id { get; set; }
    public string? Url { get; set; }
    public string? AttributedTo { get; set; }
    public string? InReplyTo { get; set; }
    public string? Context { get; set; }
    public string? Conversation { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public DateTime? Published { get; set; }
    public DateTime? Updated { get; set; }
    public IEnumerable<string> To { get; set; } = [];
    public IEnumerable<string>? Cc { get; set; }
}
