using NetTopologySuite.Geometries;

namespace Kamino.Entities;

public class Place : BasicEntity
{
    public Point? Location { get; set; }
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public SourceType SourceType { get; set; }
    public string? Source { get; set; }

    public virtual Profile? Author { get; set; }

    public virtual ICollection<Post> Posts { get; } = [];
    public virtual ICollection<Post> Tags { get; } = [];
}
