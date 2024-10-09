namespace Kamino.Shared.Entities;

public class Profile : BasicEntity
{
    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Summary { get; set; }

    public string? PublicKeyId { get; set; }

    public string? PublicKey { get; set; }

    public string? Inbox { get; set; }

    public Uri? Icon { get; set; }

    public string? PrivateKey { get; set; }

    public DateTime? CachedAt { get; set; }

    public virtual ICollection<Post> PostsAuthored { get; } = [];

    public virtual ICollection<Place> PlacesAuthored { get; } = [];

    public virtual ICollection<Ping> PingsActor { get; } = [];

    public virtual ICollection<Ping> PingsTo { get; } = [];
}
