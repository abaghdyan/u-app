using Application.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VistaLOS.Application.Data.Common.Utilities;
using VistaLOS.Application.Identity.Data.Options;
using VistaLOS.Application.Identity.Data.Repositories;

namespace VistaLOS.Application.Identity.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityDbContext(this IServiceCollection services,
        IdentityDbOptions dbOptions)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(dbOptions.ConnectionString, o => {
                o.EnableRetryOnFailure();
                o.MigrationsHistoryTable(IdentityDbContext.MigrationTableName, IdentityDbContext.GetSchemaName(dbOptions));
            }));

        return services;
    }

    public static IServiceCollection AddIdentityRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ITransactionManager<IdentityDbContext>, TransactionManager<IdentityDbContext>>();

        return services;
    }
}
