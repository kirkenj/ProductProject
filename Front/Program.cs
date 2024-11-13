using Front;
using Front.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Front.Models;
using Clients.CustomGateway;
using Clients.Contracts;
using Front.MessageHandlers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.Configure<GatewayClientSettings>(o => o.Uri = "https://localhost:7023/");

builder.Services.AddLogging();

builder.Services.AddScoped<ITokenGetter<IGatewayClient>, TokenGetter>();

builder.Services.AddScoped<HeadersMessageHandler>();
builder.Services.AddScoped<TokenDelegatingHandler>();


builder.Services.AddScoped<IAccessTokenProvider, CustomTokenAccessor>();
builder.Services.AddScoped<LocalStorageAccessor>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(s => s.GetService<CustomAuthStateProvider>() ?? throw new Exception());

builder.Services.AddHttpClient<IGatewayClient, GatewayClient>(nameof(IGatewayClient), a => a = new HttpClient() { BaseAddress = new("http://127.0.0.1:4444") }).AddHttpMessageHandler<HeadersMessageHandler>()
    .AddHttpMessageHandler<TokenDelegatingHandler>();
builder.Services.AddHttpClient<IAuthGatewayClient, AuthGatewayClient>(nameof(IAuthGatewayClient), a => a = new HttpClient() { BaseAddress = new("http://127.0.0.1:4444") });

var sp = builder.Services.BuildServiceProvider();

builder.Services.AddScoped<IAuthGatewayClient, AuthGatewayClient>();
builder.Services.AddScoped<IGatewayClient, GatewayClient>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

await app.RunAsync();