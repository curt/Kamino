using AspNetCore.Authentication.Basic;
using Fluid;
using Fluid.MvcViewEngine;
using Kamino.Endpoint;
using Kamino.Repo.Npgsql;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContextFactory<NpgsqlContext, NpgsqlContextFactory>
(
    optionsBuilder =>
    {
        // See https://stackoverflow.com/a/76697983.
        var pgsqlPassword = config["POSTGRES_PASSWORD"];
        var connectionString = $"Host=pgsqldb;Database=kamino;Username=kamino;Password={pgsqlPassword}";
        var dataSource = DbContextOptionsBuilderHelpers.CreateNpgsqlDataSourceBuilder(connectionString).Build();

        optionsBuilder.UseNpgsql
        (
            dataSource,
            npgsqlOptionsBuilder =>
            {
                npgsqlOptionsBuilder.UseNetTopologySuite();
                npgsqlOptionsBuilder.MigrationsAssembly("Kamino.Repo.Npgsql");
                npgsqlOptionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }
        );
    }
);
builder.Services.AddAuthentication(BasicDefaults.AuthenticationScheme).AddBasic<BasicUserValidationService>
(
    options => { options.Realm = "Kamino"; }
);

// Add services to the container.
builder.Services.Configure<FluidMvcViewOptions>
(
    options =>
    {
        options.TemplateOptions.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
    }
);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // This seems to contradict Microsoft's documentation at
    // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0
    // but it seems to work. See commentary: https://stackoverflow.com/a/53215249
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllersWithViews
(
    options =>
    {
        options.Filters.Add<ServiceExceptionFilter>();

        // See https://stackoverflow.com/a/59813295.
        var jsonInputFormatter = options.InputFormatters
            .OfType<SystemTextJsonInputFormatter>()
            .Single();

        jsonInputFormatter.SupportedMediaTypes.Add("application/ld+json");
        jsonInputFormatter.SupportedMediaTypes.Add("application/activity+json");
    }
)
.AddJsonOptions
(
    options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.TypeInfoResolver = new AlphabeticalOrderJsonTypeInfoResolver();
    }
)
.AddFluid();

var app = builder.Build();

app.UseForwardedHeaders();

// Migrate the database.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NpgsqlContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
