using Kamino.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Repo;

public class NpgsqlContext : Context
{
    public NpgsqlContext(DbContextOptions<NpgsqlContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresEnum<PostType>();
        modelBuilder.HasPostgresEnum<SourceType>();

        modelBuilder.Entity<Place>(e =>
            e.Property(p => p.Location).HasColumnType("geography (point)")
        );
    }
}
