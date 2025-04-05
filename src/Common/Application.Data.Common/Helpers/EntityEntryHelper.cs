using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Enums;

namespace VistaLOS.Application.Data.Common.Helpers;

public static class EntityEntryHelper
{
    public static EntityActionType GetEntityActionType(EntityState entityState)
    {
        return entityState switch {
            EntityState.Added => EntityActionType.Create,
            EntityState.Modified => EntityActionType.Update,
            EntityState.Deleted => EntityActionType.Delete,
            _ => EntityActionType.Unknown,
        };
    }
}
