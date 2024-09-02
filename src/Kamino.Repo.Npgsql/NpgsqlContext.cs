using Kamino.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Repo.Npgsql;

public class NpgsqlContext : Context
{
    public NpgsqlContext() { }

    public NpgsqlContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<SourceType>();
    }
}
