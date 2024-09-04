using Kamino.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Kamino.Repo.Npgsql;

public static class DbContextOptionsBuilderHelpers
{
    public static NpgsqlDataSourceBuilder CreateNpgsqlDataSourceBuilder(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<SourceType>();

        return dataSourceBuilder;
    }
}
