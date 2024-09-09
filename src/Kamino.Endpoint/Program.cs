using AspNetCore.Authentication.Basic;
using Fluid;
using Fluid.MvcViewEngine;
using Kamino.Endpoint;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContextFactory<ApplicationContext, ApplicationContextFactory>();
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

builder.Services.AddControllersWithViews
(
    options =>
    {
        // See https://stackoverflow.com/a/59813295
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
