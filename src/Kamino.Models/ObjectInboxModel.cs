namespace Kamino.Models;

public class ObjectInboxModel : InboxModelBase, IObjectInboxModel
{
    public IObjectInboxModel? Actor { get; set; }
    public IObjectInboxModel? Object { get; set; }
    public IObjectInboxModel? Target { get; set; }
    public IListObjectInboxModel? To { get; set; }
    public IListObjectInboxModel? Cc { get; set; }
    public IListObjectInboxModel? Bcc { get; set; }
    public string? Name { get; set; }
    public string? Href { get; set; }


    [JsonExtensionData]
    public IDictionary<string, object>? ExtraElements { get; set; }
}
