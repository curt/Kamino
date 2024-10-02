using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Repo;

public class NpgsqlContextFactory(DbContextOptions<NpgsqlContext> options)
    : IDbContextFactory<NpgsqlContext>
{
    public NpgsqlContext CreateDbContext()
    {
        return new NpgsqlContext(options);
    }
}
