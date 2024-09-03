using Kamino.Repo;
using Kamino.Repo.Npgsql;
using Kamino.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var pgsqlPassword = config["POSTGRES_PASSWORD"];

builder.Services.AddDbContext<Context, NpgsqlContext>
(
    options =>
    {
        options.UseNpgsqlContext($"Host=pgsqldb;Database=kamino;Username=kamino;Password={pgsqlPassword}");
    }
);

builder.Services.AddScoped<IPostsService, PostsService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<Context>();
        context.Database.EnsureCreated();
    }
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
