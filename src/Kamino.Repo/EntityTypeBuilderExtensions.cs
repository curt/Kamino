using Kamino.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamino.Repo;

internal static class EntityTypeBuilderExtensions
{
    internal static EntityTypeBuilder<T> IsIdentifiableEntity<T>(this EntityTypeBuilder<T> e) where T : IdentifiableEntity
    {
        e.Property(p => p.Id).HasValueGenerator<Uuid7ValueGenerator>();
        e.Property(p => p.CreatedAt).IsRequired();

        return e;
    }

    internal static EntityTypeBuilder<T> IsBasicEntity<T>(this EntityTypeBuilder<T> e) where T : BasicEntity
    {
        e.IsIdentifiableEntity();

        e.Property(p => p.Uri).IsRequired();
        e.Property(p => p.Url).IsRequired();
        e.HasIndex(p => p.Uri).IsUnique();

        return e;
    }
}
