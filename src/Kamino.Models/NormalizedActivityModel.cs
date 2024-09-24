namespace Kamino.Models;

public class NormalizedActivityModel : ActivityModelBase
{
    private Uri? _objectUri = null;

    public Uri? Id { get; set; }
    public string? Type { get; set; }
    public Uri? Actor { get; set; }
    public Uri? Target { get; set; }

    public Uri? Object
    {
        get => ObjectModel?.Id ?? _objectUri;
        set { _objectUri = value; }
    }

    [JsonIgnore]
    public NormalizedObjectModel? ObjectModel { get; set; }
}
