using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Helpers;
using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common.Entities;
using VistaLOS.Application.Data.Common.Helpers;

namespace VistaLOS.Application.Data.Common;

public abstract class CommonDbContext : DbContext
{
    protected readonly ILogger Logger;
    protected readonly IUserContextAccessor UserContextAccessor;

    public const string MigrationTableName = "__EFMigrationsHistory";

    private record EntryInfo(EntityEntry EntityEntry, EntityState InitialState);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changedEntriesData = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Unchanged)
            .Select(e => new EntryInfo(e, e.State))
            .ToList();

        SetTrackingProperties();
        var result = await base.SaveChangesAsync(cancellationToken);
        LogChangedEntitiesActions(changedEntriesData);

        return result;
    }

    private void SetTrackingProperties()
    {
        var now = DateTime.UtcNow;

        foreach (var entityAccessor in ChangeTracker.Entries()
                     .Where(e => e.Entity is AbstractEntity && e.State != EntityState.Unchanged)) {
            var entityInstance = (entityAccessor.Entity as AbstractEntity)!;

            entityInstance.ModifiedOn = now;

            if (entityInstance.CreatedOn == default) {
                entityInstance.CreatedOn = now;
            }

            if (entityAccessor.State == EntityState.Deleted) {
                entityInstance.DeletedDate = now;
                entityAccessor.State = EntityState.Modified;
            }
        }
    }

    public void Clear()
    {
        foreach (var entry in ChangeTracker.Entries()) {
            entry.State = EntityState.Detached;
        }
    }

    private void LogChangedEntitiesActions(IEnumerable<EntryInfo> changedEntries)
    {
        using var _ = LogHelper.SetModuleScope(LoggingModules.EntityChange);
        foreach (var entryPair in changedEntries.Where(e => e.EntityEntry.Entity is IIdentifiable<object>)) {
            var identifiableEntity = entryPair.EntityEntry.Entity as IIdentifiable<object>;
            var entityType = entryPair.EntityEntry.Metadata.ClrType.Name;
            var actionType = EntityEntryHelper.GetEntityActionType(entryPair.InitialState);

            Logger.SetEntityId(identifiableEntity?.Id.ToString())
                  .SetEntityType(entityType)
                  .SetEntityActionType(actionType)
                  .Information("An action to create or modify an entity has been detected.");
        }
    }

    public CommonDbContext(DbContextOptions options,
        IUserContextAccessor userContextAccessor,
        ILogger logger)
        : base(options)
    {
        UserContextAccessor = userContextAccessor;
        Logger = logger;
    }
}
