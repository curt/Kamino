using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint;

public class ApplicationContextFactory(DbContextOptions<ApplicationContext> options, IConfiguration config) :
    IDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext()
    {
        return new ApplicationContext(options, config);
    }
}
