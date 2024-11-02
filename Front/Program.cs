using Front;
using Front.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Front.Models;
using Clients.CustomGateway;
using Clients.Contracts;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddScoped<ITokenGetter<IGatewayClient>, TokenGetter>();

builder.Services.AddScoped<LocalStorageAccessor>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


builder.Services.Configure<GatewayClientSettings>(o => o.Uri = "https://localhost:7023/");

builder.Services.AddScoped<IAuthGatewayClient, AuthGatewayClient>();
builder.Services.AddScoped<IGatewayClient, GatewayClient>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

await app.RunAsync();