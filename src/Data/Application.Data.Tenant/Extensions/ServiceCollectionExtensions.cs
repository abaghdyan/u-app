using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common.Utilities;
using VistaLOS.Application.Data.Tenant.Repositories;

namespace VistaLOS.Application.Data.Tenant.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTenantDbContext(this IServiceCollection services)
    {
        services.AddDbContext<TenantDbContext>((p, b) => {
            var userContextAccessor = p.GetRequiredService<IUserContextAccessor>();
            var userContext = userContextAccessor.GetUserContext();
            if (userContext != null) {
                b.UseSqlServer(userContext.ConnectionString, o => {
                    o.EnableRetryOnFailure();
                    o.MigrationsHistoryTable(TenantDbContext.MigrationTableName, TenantDbContext.GetSchemaName());
                });
            }
        });

        return services;
    }

    public static IServiceCollection AddTenantRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        
        services.AddScoped<ITransactionManager<TenantDbContext>, TransactionManager<TenantDbContext>>();

        return services;
    }
}
