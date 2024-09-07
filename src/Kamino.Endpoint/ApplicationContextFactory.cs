using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint;

public class ApplicationContextFactory(DbContextOptions<ApplicationContext> options) : IDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext()
    {
        return new ApplicationContext(options);
    }
}
