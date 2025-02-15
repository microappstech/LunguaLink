using Langua.WebUI.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Langua.WebUI.Client.Services;
using Microsoft.Extensions.Localization;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddRadzenComponents();
//builder.Services.AddLocalization();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<LangClientService>();
builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
