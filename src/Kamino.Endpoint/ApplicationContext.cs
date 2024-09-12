using Kamino.Repo.Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint;

public class ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration config) : NpgsqlContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);

        // See https://stackoverflow.com/a/76697983.
        var pgsqlPassword = config["POSTGRES_PASSWORD"];
        var connectionString = $"Host=pgsqldb;Database=kamino;Username=kamino;Password={pgsqlPassword}";
        var dataSource = DbContextOptionsBuilderHelpers.CreateNpgsqlDataSourceBuilder(connectionString).Build();

        builder.UseNpgsql
        (
            dataSource,
            options =>
            {
                options.UseNetTopologySuite();
                options.MigrationsAssembly(typeof(NpgsqlContext).Assembly.FullName);
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }
        );
    }
}
