using Kamino.Admin.Client.Pages;
using Kamino.Admin.Components;
using Kamino.Admin.Components.Account;
using Kamino.Shared.Entities;
using Kamino.Shared.Repo;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<
    AuthenticationStateProvider,
    PersistingServerAuthenticationStateProvider
>();

builder.Services.AddAuthorization();
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddDbContext<NpgsqlContext>(optionsBuilder =>
{
    // See https://stackoverflow.com/a/76697983.
    var pgsqlPassword = builder.Configuration["POSTGRES_PASSWORD"];
    var connectionString = $"Host=pgsqldb;Database=kamino;Username=kamino;Password={pgsqlPassword}";
    var dataSource = DbContextOptionsBuilderHelpers
        .CreateNpgsqlDataSourceBuilder(connectionString)
        .Build();

    optionsBuilder.UseNpgsql(
        dataSource,
        npgsqlOptionsBuilder =>
        {
            npgsqlOptionsBuilder.UseNetTopologySuite();
            npgsqlOptionsBuilder.MigrationsAssembly("Kamino.Shared");
            npgsqlOptionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }
    );
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services.AddIdentityCore<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = true
    )
    .AddEntityFrameworkStores<NpgsqlContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Kamino.Admin.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
