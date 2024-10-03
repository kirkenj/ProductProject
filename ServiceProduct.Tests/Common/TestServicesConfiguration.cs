﻿using Application.Contracts.AuthService;
using Application.Contracts.Persistence;
using Application.DTOs.Product;
using Application.Features.Product.Handlers.Commands;
using Application.MediatRBehaviors;
using Application.Models.UserClient;
using Cache.Contracts;
using Cache.Models;
using Clients.AuthApi;
using EmailSender.Contracts;
using FluentValidation;
using Infrastucture.AuthClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Repositories;
using System.Reflection;
using Tools;

namespace ServiceProduct.Tests.Common
{
    public static class TestServicesConfiguration
    {
        public static IServiceCollection ConfigureTestServices(this IServiceCollection services)
        {
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.AddAutoMapper(typeof(ProductDto).Assembly);

            services.AddAutoMapper(a => a.CreateMap<UserDto, AuthClientUser>().ReverseMap());
            services.AddAutoMapper(a => a.CreateMap<RoleDto, AuthClientRole>().ReverseMap());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(UpdateProductComandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(UpdateProductComandHandler).Assembly);

            services.Configure<CustomCacheOptions>(a => a.ConnectionUri = "localhost:3330");

            services.AddLogging();
            services.AddSingleton<ICustomMemoryCache, RedisCustomMemoryCacheWithEvents>();
            services.AddSingleton<IEmailSender, TestEmailSender>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAuthApiClient, TestAuthClient>();
            services.AddScoped<IAuthApiClientService, AuthClientService>();

            return services;
        }
    }
}
