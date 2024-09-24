namespace Kamino.Models;

public class ObjectInboxModel : InboxModelBase, IObjectInboxModel
{
    public IObjectInboxModel? Actor { get; set; }
    public IObjectInboxModel? Object { get; set; }
    public IObjectInboxModel? Target { get; set; }
    public IListObjectInboxModel? To { get; set; }
    public IListObjectInboxModel? Cc { get; set; }
    public IListObjectInboxModel? Bcc { get; set; }
    public IListObjectInboxModel? Followers { get; set; }
    public IListObjectInboxModel? Following { get; set; }
    public string? Name { get; set; }
    public string? Href { get; set; }
    public string? Inbox { get; set; }
    public string? Outbox { get; set; }
    public string? Url { get; set; }
    public string? Summary { get; set; }
    public string? PreferredUsername { get; set; }
    public PublicKeyModel? PublicKey { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object>? ExtraElements { get; set; }
}
