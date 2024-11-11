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


builder.Services.AddScoped<ITokenGetter<IGatewayClient>, TokenGetter>();

builder.Services.AddScoped<HeadersMessageHandler>();
builder.Services.AddScoped<TokenDelegatingHandler>();


builder.Services.AddScoped<IAccessTokenProvider, CustomTokenAccessor>();
builder.Services.AddScoped<LocalStorageAccessor>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

//builder.Services.AddScoped<HttpClient>();

builder.Services.AddHttpClient<IGatewayClient, GatewayClient2>(nameof(IGatewayClient), a => a = new HttpClient() { BaseAddress = new("http://127.0.0.1:4444") }).AddHttpMessageHandler<HeadersMessageHandler>()
    .AddHttpMessageHandler<TokenDelegatingHandler>();
builder.Services.AddHttpClient<IAuthGatewayClient, AuthGatewayClient>(nameof(IAuthGatewayClient), a => a = new HttpClient() { BaseAddress = new("http://127.0.0.1:4444") });

var sp = builder.Services.BuildServiceProvider();

builder.Services.AddScoped<IAuthGatewayClient, AuthGatewayClient>();
builder.Services.AddScoped<IGatewayClient, GatewayClient2>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

await app.RunAsync();


//System.InvalidOperationException: The 'AuthorizationMessageHandler' is not configured.
//Call 'ConfigureHandler' and provide a list of endpoint urls to attach the token to.