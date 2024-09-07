using Kamino.Repo.Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : NpgsqlContext(options) { }
