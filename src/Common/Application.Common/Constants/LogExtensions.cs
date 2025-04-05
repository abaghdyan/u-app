using Serilog;
using VistaLOS.Application.Common.Enums;

namespace VistaLOS.Application.Common.Constants;

public static class LogExtensions
{
    private const string EntityIdPropertyName = "EntityId";
    private const string EntityTypePropertyName = "EntityType";
    private const string EntityActionTypePropertyName = "EntityActionType";

    public static ILogger SetEntityId(this ILogger logger, string? entityId) =>
        logger.SetProperty(EntityIdPropertyName, entityId);

    public static ILogger SetEntityType(this ILogger logger, string? entityType) =>
        logger.SetProperty(EntityTypePropertyName, entityType);

    public static ILogger SetEntityActionType(this ILogger logger, EntityActionType? actionType) =>
        logger.SetProperty(EntityActionTypePropertyName, actionType?.ToString());

    public static ILogger SetProperty(this ILogger logger, string name, object? value)
    {
        if (value != null) {
            return logger.ForContext(name, value);
        }

        return logger;
    }
}

