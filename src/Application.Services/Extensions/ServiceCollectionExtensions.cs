using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Impl;
using VistaLOS.Application.Services.Options;

namespace VistaLOS.Application.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, RedisOptions redisOptions)
    {
        services.AddStackExchangeRedisCache(options => {
            options.Configuration = redisOptions.ConnectionString;
        });

        services.AddSingleton<IConnectionMultiplexer>(_
            => ConnectionMultiplexer.Connect(redisOptions.ConnectionString));

        return services;
    }

    public static IServiceCollection AddApplicationServicesLayer(this IServiceCollection services)
    {
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IPlanLimitationService, PlanLimitationService>();
        services.AddSingleton<CacheConnector>();

        return services;
    }
}
