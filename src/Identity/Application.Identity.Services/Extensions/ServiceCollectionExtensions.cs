using Microsoft.Extensions.DependencyInjection;
using VistaLOS.Application.Identity.Services.Abstraction;
using VistaLOS.Application.Identity.Services.Managers;
using VistaLOS.Application.Identity.Services.Mapping;

namespace VistaLOS.Application.Identity.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityMappingProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IdentityMappingProfile));

        return services;
    }

    public static IServiceCollection AddIdentityServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, Impl.AuthenticationService>();
        services.AddScoped<IdentityRoleManager>();
        services.AddScoped<IdentityUserManager>();

        return services;
    }
}
