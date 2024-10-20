using Kamino.Admin.Client;
using Kamino.Admin.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder
    .Services.AddScoped(sp => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    })
    .AddMudServices()
    .AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>()
    .AddTransient<IPingsService, PingsClientService>();

await builder.Build().RunAsync();
