using Front;
using Front.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Clients.CustomGateway;
using Front.MessageHandlers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.Configure<GatewayClientSettings>(o => o.Uri = "https://localhost:7023/");

builder.Services.AddLogging();

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
builder.Services.AddScoped<IGatewayClient, GatewayClient>(sp => 
{
    var clf = sp.GetRequiredService<IHttpClientFactory>();
    var client = clf.CreateClient(nameof(IGatewayClient));
    var settings = sp.GetService<IOptions<GatewayClientSettings>>() ?? throw new Exception("IOptions<GatewayClientSettings> is null");
    return new GatewayClient(settings.Value.Uri, client);
});

builder.Services.AddAuthorizationCore();

var app = builder.Build();

await app.RunAsync();