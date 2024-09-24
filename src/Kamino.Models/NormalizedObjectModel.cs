using System;

namespace Kamino.Models;

public class NormalizedObjectModel
{
    public Uri? Id { get; set; }
    public string? Summary { get; set; }
    public Uri? Url { get; set; }
    public Uri? AttributedTo { get; set; }
    public Uri? InReplyTo { get; set; }
    public Uri? Context { get; set; }
    public Uri? Conversation { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public DateTime? Published { get; set; }
    public DateTime? Updated { get; set; }
    public IEnumerable<string> To { get; set; } = [];
    public IEnumerable<string> Cc { get; set; } = [];
}
