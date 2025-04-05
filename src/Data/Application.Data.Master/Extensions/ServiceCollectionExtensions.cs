using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VistaLOS.Application.Data.Common.Utilities;
using VistaLOS.Application.Data.Master.Options;
using VistaLOS.Application.Data.Master.Repositories;

namespace VistaLOS.Application.Data.Master.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasterDbContext(this IServiceCollection services,
        MasterDbOptions dbOptions)
    {
        services.AddDbContext<MasterDbContext>(options =>
            options.UseSqlServer(dbOptions.ConnectionString, o => {
                o.EnableRetryOnFailure();
                o.MigrationsHistoryTable(MasterDbContext.MigrationTableName, MasterDbContext.GetSchemaName(dbOptions));
            }));

        return services;
    }

    public static IServiceCollection AddMasterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantStorageRepository, TenantStorageRepository>();

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        services.AddScoped<ITransactionManager<MasterDbContext>, TransactionManager<MasterDbContext>>();

        return services;
    }
}
