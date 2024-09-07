using Kamino.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Kamino.Repo.Npgsql;

public static class DbContextOptionsBuilderHelpers
{
    public static NpgsqlDataSourceBuilder CreateNpgsqlDataSourceBuilder(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        dataSourceBuilder.MapEnum<PostType>();
        dataSourceBuilder.MapEnum<SourceType>();

        return dataSourceBuilder;
    }
}
