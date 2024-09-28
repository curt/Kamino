namespace Kamino.Entities;

public class Pong : IdentifiableEntity
{
    public Uri? ActivityUri { get; set; }
    public Ping? Ping { get; set; }
}
