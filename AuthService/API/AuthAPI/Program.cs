using Application;
using AuthAPI.Middlewares;
using AuthAPI.Registrations;
using Constants;
using Infrastructure;
using Persistence;

Console.WriteLine("Application started");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.ConfigureApiServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureJwtAuthentication();


builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApiConstants.ADMIN_POLICY_NAME, policy =>
    {
        policy.RequireRole(ApiConstants.ADMIN_ROLE_NAME);
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
