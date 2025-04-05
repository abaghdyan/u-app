using Microsoft.Extensions.DependencyInjection;
using VistaLOS.Application.Common.Multitenancy;

namespace VistaLOS.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMultitenancy(this IServiceCollection services)
    {
        services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        services.AddScoped<IUserContextWriter, UserContextWriter>();
        services.AddScoped<UserContextHolder>();

        return services;
    }
}
