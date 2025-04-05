using System.Diagnostics;
using Application.Identity.Data;
using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Helpers;
using VistaLOS.Application.Data.Master.Options;
using VistaLOS.Application.Data.Tenant;

namespace VistaLOS.Application.Api.HostedServices;

public class MigrationHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await MigrateMasterDbContextAsync();
        await MigrateIdentityDbContextAsync();
        await MigrateTenantDbContextsAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task MigrateMasterDbContextAsync()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var masterDbContext = serviceScope.ServiceProvider.GetRequiredService<MasterDbContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger>().ForContext<MasterDbContext>();
        
        var migrationTime = Stopwatch.StartNew();
        logger.Information("Master storage migration started");
        
        try {
            await masterDbContext.Database.MigrateAsync();
        }
        catch (Exception ex) {
            logger.Error(ex, $"Master storage migration failed. " +
                $"Message: {ex.Message ?? ex.InnerException?.Message}. ExceptionType: {ex.GetType().FullName}");
            throw;
        }

        migrationTime.Stop();
        logger.Information($"Master storage migration finished successfully. Duration = {migrationTime.ElapsedMilliseconds}.");
    }

    public async Task MigrateTenantDbContextsAsync()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var masterDbContext = serviceScope.ServiceProvider.GetRequiredService<MasterDbContext>();
        var options = serviceScope.ServiceProvider.GetRequiredService<MasterDbOptions>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger>().ForContext<TenantDbContext>();
        
        var migrationTime = Stopwatch.StartNew();
        logger.Information("All tenant storages migration started");

        var tenantStorages = await masterDbContext.TenantStorages
            .Include(ts => ts.Tenants)
            .ToListAsync();

        foreach (var tenantStorage in tenantStorages) {
            try {
                foreach (var tenant in tenantStorage.Tenants) {
                    tenant.Status = TenantStatus.Suspended;
                }
                masterDbContext.Tenants.UpdateRange(tenantStorage.Tenants);
                await masterDbContext.SaveChangesAsync();

                using var scope = serviceScope.ServiceProvider.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                var userContextWriter = scope.ServiceProvider.GetRequiredService<IUserContextWriter>();
                var connectionBuilder = ConnectionHelper.GetConnectionBuilder(options.EncryptionKey, tenantStorage);
                var userContext = UserContext.Create(null, default, default, connectionBuilder.ToString());
                userContextWriter.SetUserContext(userContext);

                var tenantDbContext = serviceProvider.GetRequiredService<TenantDbContext>();

                await tenantDbContext.Database.MigrateAsync();

                foreach (var tenant in tenantStorage.Tenants) {
                    tenant.Status = TenantStatus.Active;
                }
                masterDbContext.Tenants.UpdateRange(tenantStorage.Tenants);
                await masterDbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                logger.Error(ex,
                    $"'{tenantStorage.StorageName}' tenant storage migration failed. " +
                    $"Message: {ex.Message ?? ex.InnerException?.Message}. ExceptionType: {ex.GetType().FullName}");
            }
        }

        migrationTime.Stop();
        logger.Information($"All tenant storages migration finished", new { Duration = migrationTime.ElapsedMilliseconds });
    }

    public async Task MigrateIdentityDbContextAsync()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var identityDbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger>().ForContext<IdentityDbContext>();
        
        var migrationTime = Stopwatch.StartNew();
        logger.Information($"Identity storage migration started");
        
        try {
            await identityDbContext.Database.MigrateAsync();
        }
        catch (Exception ex) {
            logger.Error(ex, $"Identity storage migration failed. " +
                $"Message: {ex.Message ?? ex.InnerException?.Message}. ExceptionType: {ex.GetType().FullName}");
            throw;
        }

        migrationTime.Stop();
        logger.Information($"Identity storage migration finished successfully. Duration = {migrationTime.ElapsedMilliseconds}.");
    }
}
