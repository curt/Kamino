using Microsoft.EntityFrameworkCore;

namespace Kamino.Repo.Npgsql;

public class NpgsqlContextFactory(DbContextOptions<NpgsqlContext> options) :
    IDbContextFactory<NpgsqlContext>
{
    public NpgsqlContext CreateDbContext()
    {
        return new NpgsqlContext(options);
    }
}
