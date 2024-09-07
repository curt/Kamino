using Fluid;
using Fluid.MvcViewEngine;
using Kamino.Endpoint;
using Kamino.Repo.Npgsql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

{
    // See https://stackoverflow.com/a/76697983.
    var pgsqlPassword = config["POSTGRES_PASSWORD"];
    var connectionString = $"Host=pgsqldb;Database=kamino;Username=kamino;Password={pgsqlPassword}";
    var dataSource = DbContextOptionsBuilderHelpers.CreateNpgsqlDataSourceBuilder(connectionString).Build();
    builder.Services.AddDbContextFactory<ApplicationContext, ApplicationContextFactory>
    (
        options => { options.UseNpgsql(dataSource, options => options.UseNetTopologySuite()); }
    );
}

// Add services to the container.
builder.Services.Configure<FluidMvcViewOptions>
(
    options =>
    {
        options.TemplateOptions.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
    }
);
builder.Services.AddControllersWithViews().AddFluid();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
