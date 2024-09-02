using Kamino.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Repo;

public abstract class Context : DbContext
{
    public Context() { }

    public Context(DbContextOptions options) : base(options) { }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasOne(post => post.Author).WithMany(profile => profile.Authored).IsRequired();
        modelBuilder.Entity<Post>(e => { e.Property(p => p.Id).HasValueGenerator<Uuid7ValueGenerator>(); });
        modelBuilder.Entity<Post>(e => { e.Property(p => p.Uri).IsRequired(); });
        modelBuilder.Entity<Post>(e => { e.HasIndex(p => p.Uri).IsUnique(); });
        modelBuilder.Entity<Post>(e => { e.Property(p => p.Url).IsRequired(); });

        modelBuilder.Entity<Profile>(e => { e.Property(p => p.Id).HasValueGenerator<Uuid7ValueGenerator>(); });
        modelBuilder.Entity<Profile>(e => { e.Property(p => p.Uri).IsRequired(); });
        modelBuilder.Entity<Profile>(e => { e.HasIndex(p => p.Uri).IsUnique(); });
        modelBuilder.Entity<Profile>(e => { e.Property(p => p.Url).IsRequired(); });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TimestampsInterceptor());
    }
}
