using Kamino.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Kamino.Repo.Npgsql;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseNpgsqlContext(this DbContextOptionsBuilder builder, string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<SourceType>();

        var dataSource = dataSourceBuilder.Build();
        builder.UseNpgsql(dataSource);

        return builder;
    }
}
